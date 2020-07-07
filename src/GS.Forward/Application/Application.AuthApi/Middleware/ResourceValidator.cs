using IdentityServer4.Models;
using IdentityServer4.Stores;
using IdentityServer4.Validation;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Application.AuthApi.Middleware
{
	/// <summary>
	/// 默认实现源码.
	/// source : IdentityServer4.Validation.DefaultResourceValidator
	/// </summary>
	public class ResourceValidator : IResourceValidator
	{
		private readonly ILogger _logger;

		private readonly IScopeParser _scopeParser;

		private readonly IResourceStore _store;

		public ResourceValidator(IResourceStore store, IScopeParser scopeParser, ILogger<DefaultResourceValidator> logger)
		{
			_logger = logger;
			_scopeParser = scopeParser;
			_store = store;
		}

		public virtual async Task<ResourceValidationResult> ValidateRequestedResourcesAsync(ResourceValidationRequest request)
		{
			if (request == null)
			{
				throw new ArgumentNullException("request");
			}
			ParsedScopesResult parsedScopesResult = _scopeParser.ParseScopeValues(request.Scopes);
			ResourceValidationResult result = new ResourceValidationResult();
			if (!parsedScopesResult.Succeeded)
			{
				foreach (ParsedScopeValidationError error in parsedScopesResult.Errors)
				{
					_logger.LogError("Invalid parsed scope {scope}, message: {error}", error.RawValue, error.Error);
					result.InvalidScopes.Add(error.RawValue);
				}
				return result;
			}
			string[] scopeNames = parsedScopesResult.ParsedScopes.Select((ParsedScopeValue x) => x.ParsedName).Distinct().ToArray();
			Resources resourcesFromStore = await _store.FindEnabledResourcesByScopeAsync(scopeNames);
			foreach (ParsedScopeValue parsedScope in parsedScopesResult.ParsedScopes)
			{
				await ValidateScopeAsync(request.Client, resourcesFromStore, parsedScope, result);
			}
			if (result.InvalidScopes.Count > 0)
			{
				result.Resources.IdentityResources.Clear();
				result.Resources.ApiResources.Clear();
				result.Resources.ApiScopes.Clear();
				result.ParsedScopes.Clear();
			}
			return result;
		}

		protected virtual async Task ValidateScopeAsync(Client client, Resources resourcesFromStore, ParsedScopeValue requestedScope, ResourceValidationResult result)
		{
			if (requestedScope.ParsedName == "offline_access")
			{
				if (await IsClientAllowedOfflineAccessAsync(client))
				{
					result.Resources.OfflineAccess = true;
					result.ParsedScopes.Add(new ParsedScopeValue("offline_access"));
				}
				else
				{
					result.InvalidScopes.Add("offline_access");
				}
				return;
			}
			IdentityResource identity = resourcesFromStore.FindIdentityResourcesByScope(requestedScope.ParsedName);
			if (identity != null)
			{
				if (await IsClientAllowedIdentityResourceAsync(client, identity))
				{
					result.ParsedScopes.Add(requestedScope);
					result.Resources.IdentityResources.Add(identity);
				}
				else
				{
					result.InvalidScopes.Add(requestedScope.RawValue);
				}
				return;
			}
			ApiScope apiScope = resourcesFromStore.FindApiScope(requestedScope.ParsedName);
			if (apiScope != null)
			{
				if (await IsClientAllowedApiScopeAsync(client, apiScope))
				{
					result.ParsedScopes.Add(requestedScope);
					result.Resources.ApiScopes.Add(apiScope);
					foreach (ApiResource item in resourcesFromStore.FindApiResourcesByScope(apiScope.Name))
					{
						result.Resources.ApiResources.Add(item);
					}
				}
				else
				{
					result.InvalidScopes.Add(requestedScope.RawValue);
				}
			}
			else
			{
				_logger.LogError("Scope {scope} not found in store.", requestedScope.ParsedName);
				result.InvalidScopes.Add(requestedScope.RawValue);
			}
		}

		protected virtual Task<bool> IsClientAllowedIdentityResourceAsync(Client client, IdentityResource identity)
		{
			bool flag = client.AllowedScopes.Contains(identity.Name);
			if (!flag)
			{
				_logger.LogError("Client {client} is not allowed access to scope {scope}.", client.ClientId, identity.Name);
			}
			return Task.FromResult(flag);
		}

		protected virtual Task<bool> IsClientAllowedApiScopeAsync(Client client, ApiScope apiScope)
		{
			bool flag = client.AllowedScopes.Contains(apiScope.Name);
			if (!flag)
			{
				_logger.LogError("Client {client} is not allowed access to scope {scope}.", client.ClientId, apiScope.Name);
			}
			return Task.FromResult(flag);
		}

		protected virtual Task<bool> IsClientAllowedOfflineAccessAsync(Client client)
		{
			bool allowOfflineAccess = client.AllowOfflineAccess;
			if (!allowOfflineAccess)
			{
				_logger.LogError("Client {client} is not allowed access to scope offline_access (via AllowOfflineAccess setting).", client.ClientId);
			}
			return Task.FromResult(allowOfflineAccess);
		}
	}

}
