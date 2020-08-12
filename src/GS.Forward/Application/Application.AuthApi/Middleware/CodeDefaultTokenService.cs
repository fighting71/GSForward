using IdentityModel;
using IdentityServer4.Configuration;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Application.AuthApi.Middleware
{
	/// <summary>
	/// source:IdentityServer4.Services.DefaultTokenService
	/// </summary>
	public class CodeDefaultTokenService : ITokenService
    {

		protected readonly ILogger Logger;

		protected readonly IHttpContextAccessor ContextAccessor;

		protected readonly IClaimsService ClaimsProvider;

		protected readonly IReferenceTokenStore ReferenceTokenStore;

		protected readonly ITokenCreationService CreationService;

		protected readonly ISystemClock Clock;

		protected readonly IKeyMaterialService KeyMaterialService;

		protected readonly IdentityServerOptions Options;

		public CodeDefaultTokenService(IClaimsService claimsProvider, IReferenceTokenStore referenceTokenStore, ITokenCreationService creationService, IHttpContextAccessor contextAccessor, ISystemClock clock, IKeyMaterialService keyMaterialService, IdentityServerOptions options, ILogger<DefaultTokenService> logger)
		{
			ContextAccessor = contextAccessor;
			ClaimsProvider = claimsProvider;
			ReferenceTokenStore = referenceTokenStore;
			CreationService = creationService;
			Clock = clock;
			KeyMaterialService = keyMaterialService;
			Options = options;
			Logger = logger;
		}

		public virtual async Task<Token> CreateIdentityTokenAsync(TokenCreationRequest request)
		{
			Logger.LogTrace("Creating identity token");
			request.Validate();
			string algorithm = ((await KeyMaterialService.GetSigningCredentialsAsync(request.ValidatedRequest.Client.AllowedIdentityTokenSigningAlgorithms)) ?? throw new InvalidOperationException("No signing credential is configured.")).Algorithm;
			List<Claim> claims = new List<Claim>();
			if (request.Nonce.IsPresent())
			{
				claims.Add(new Claim("nonce", request.Nonce));
			}
			claims.Add(new Claim("iat", Clock.UtcNow.ToUnixTimeSeconds().ToString(), "http://www.w3.org/2001/XMLSchema#integer64"));
			if (request.AccessTokenToHash.IsPresent())
			{
				claims.Add(new Claim("at_hash", CryptoHelper.CreateHashClaimValue(request.AccessTokenToHash, algorithm)));
			}
			if (request.AuthorizationCodeToHash.IsPresent())
			{
				claims.Add(new Claim("c_hash", CryptoHelper.CreateHashClaimValue(request.AuthorizationCodeToHash, algorithm)));
			}
			if (request.StateHash.IsPresent())
			{
				claims.Add(new Claim("s_hash", request.StateHash));
			}
			if (request.ValidatedRequest.SessionId.IsPresent())
			{
				claims.Add(new Claim("sid", request.ValidatedRequest.SessionId));
			}
			List<Claim> list = claims;
			list.AddRange(await ClaimsProvider.GetIdentityTokenClaimsAsync(request.Subject, request.ValidatedResources, request.IncludeAllIdentityClaims, request.ValidatedRequest));
			string identityServerIssuerUri = ContextAccessor.HttpContext.GetIdentityServerIssuerUri();
			return new Token("id_token")
			{
				CreationTime = Clock.UtcNow.UtcDateTime,
				Audiences =
			{
				request.ValidatedRequest.Client.ClientId
			},
				Issuer = identityServerIssuerUri,
				Lifetime = request.ValidatedRequest.Client.IdentityTokenLifetime,
				Claims = claims.Distinct(new ClaimComparer()).ToList(),
				ClientId = request.ValidatedRequest.Client.ClientId,
				AccessTokenType = request.ValidatedRequest.AccessTokenType,
				AllowedSigningAlgorithms = request.ValidatedRequest.Client.AllowedIdentityTokenSigningAlgorithms
			};
		}

		public virtual async Task<Token> CreateAccessTokenAsync(TokenCreationRequest request)
		{
			Logger.LogTrace("Creating access token");
			request.Validate();
			List<Claim> claims = new List<Claim>();
			List<Claim> list = claims;
			list.AddRange(await ClaimsProvider.GetAccessTokenClaimsAsync(request.Subject, request.ValidatedResources, request.ValidatedRequest));
			if (request.ValidatedRequest.Client.IncludeJwtId)
			{
				claims.Add(new Claim("jti", CryptoRandom.CreateUniqueId(16, CryptoRandom.OutputFormat.Hex)));
			}
			if (request.ValidatedRequest.SessionId.IsPresent())
			{
				claims.Add(new Claim("sid", request.ValidatedRequest.SessionId));
			}
			claims.Add(new Claim("iat", Clock.UtcNow.ToUnixTimeSeconds().ToString(), "http://www.w3.org/2001/XMLSchema#integer64"));
			string identityServerIssuerUri = ContextAccessor.HttpContext.GetIdentityServerIssuerUri();
			Token token = new Token("access_token")
			{
				CreationTime = Clock.UtcNow.UtcDateTime,
				Issuer = identityServerIssuerUri,
				Lifetime = request.ValidatedRequest.AccessTokenLifetime,
				Claims = claims.Distinct(new ClaimComparer()).ToList(),
				ClientId = request.ValidatedRequest.Client.ClientId,
				Description = request.Description,
				AccessTokenType = request.ValidatedRequest.AccessTokenType,
				AllowedSigningAlgorithms = request.ValidatedResources.Resources.ApiResources.FindMatchingSigningAlgorithms()
			};
			foreach (string item in request.ValidatedResources.Resources.ApiResources.Select((ApiResource x) => x.Name).Distinct())
			{
				token.Audiences.Add(item);
			}
			if (Options.EmitStaticAudienceClaim)
			{
				token.Audiences.Add($"{identityServerIssuerUri.EnsureTrailingSlash()}resources");
			}
			if (request.ValidatedRequest.Confirmation.IsPresent())
			{
				token.Confirmation = request.ValidatedRequest.Confirmation;
			}
			else if (Options.MutualTls.AlwaysEmitConfirmationClaim)
			{
				X509Certificate2 x509Certificate = await ContextAccessor.HttpContext.Connection.GetClientCertificateAsync();
				if (x509Certificate != null)
				{
					token.Confirmation = x509Certificate.CreateThumbprintCnf();
				}
			}
			return token;
		}

		public virtual async Task<string> CreateSecurityTokenAsync(Token token)
		{
			string result;
			if (token.Type == "access_token")
			{
				if (token.AccessTokenType == AccessTokenType.Jwt)
				{
					Logger.LogTrace("Creating JWT access token");
					result = await CreationService.CreateTokenAsync(token);
				}
				else
				{
					Logger.LogTrace("Creating reference access token");
					result = await ReferenceTokenStore.StoreReferenceTokenAsync(token);
				}
			}
			else
			{
				if (!(token.Type == "id_token"))
				{
					throw new InvalidOperationException("Invalid token type.");
				}
				Logger.LogTrace("Creating JWT identity token");
				result = await CreationService.CreateTokenAsync(token);
			}
			return result;
		}

	}
}
