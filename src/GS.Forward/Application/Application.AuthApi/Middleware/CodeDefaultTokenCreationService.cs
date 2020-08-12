using IdentityModel;
using IdentityServer4.Configuration;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Application.AuthApi.Middleware
{
	/// <summary>
	/// source: IdentityServer4.Services.DefaultTokenCreationService
	/// </summary>
	public class CodeDefaultTokenCreationService : ITokenCreationService
	{

		protected readonly IKeyMaterialService Keys;

		protected readonly ILogger Logger;

		protected readonly ISystemClock Clock;

		protected readonly IdentityServerOptions Options;

		public CodeDefaultTokenCreationService(ISystemClock clock, IKeyMaterialService keys, IdentityServerOptions options, ILogger<CodeDefaultTokenCreationService> logger)
		{
			Clock = clock;
			Keys = keys;
			Options = options;
			Logger = logger;
		}

		public virtual async Task<string> CreateTokenAsync(Token token)
		{
			return await CreateJwtAsync(new JwtSecurityToken(await CreateHeaderAsync(token), await CreatePayloadAsync(token)));
		}

		protected virtual async Task<JwtHeader> CreateHeaderAsync(Token token)
		{
			SigningCredentials obj = (await Keys.GetSigningCredentialsAsync(token.AllowedSigningAlgorithms)) ?? throw new InvalidOperationException("No signing credential is configured. Can't create JWT token");
			JwtHeader jwtHeader = new JwtHeader(obj);
			X509SecurityKey x509SecurityKey = obj.Key as X509SecurityKey;
			if (x509SecurityKey != null)
			{
				X509Certificate2 certificate = x509SecurityKey.Certificate;
				if (Clock.UtcNow.UtcDateTime > certificate.NotAfter)
				{
					Logger.LogWarning("Certificate {subjectName} has expired on {expiration}", certificate.Subject, certificate.NotAfter.ToString(CultureInfo.InvariantCulture));
				}
				jwtHeader["x5t"] = Base64Url.Encode(certificate.GetCertHash());
            }
            if (token.Type == "access_token" && !string.IsNullOrWhiteSpace(Options.AccessTokenJwtType))
            {
				jwtHeader["typ"] = Options.AccessTokenJwtType;
			}
			return jwtHeader;
		}

		protected virtual Task<JwtPayload> CreatePayloadAsync(Token token)
		{
			return Task.FromResult(token.CreateJwtPayload(Clock, Options, Logger));
		}

		protected virtual Task<string> CreateJwtAsync(JwtSecurityToken jwt)
		{
			return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(jwt));
		}

	}
}
