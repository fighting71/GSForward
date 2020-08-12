using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Application.AuthApi.Middleware
{
	/// <summary>
	/// source : Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerPostConfigureOptions
	/// </summary>
	public class CodeJwtBearerPostConfigureOptions : IPostConfigureOptions<JwtBearerOptions>
	{
		public void PostConfigure(string name, JwtBearerOptions options)
		{
			if (string.IsNullOrEmpty(options.TokenValidationParameters.ValidAudience) && !string.IsNullOrEmpty(options.Audience))
			{
				options.TokenValidationParameters.ValidAudience = options.Audience;
			}
			if (options.ConfigurationManager != null)
			{
				return;
			}
			if (options.Configuration != null)
			{
				options.ConfigurationManager = new StaticConfigurationManager<OpenIdConnectConfiguration>(options.Configuration);
			}
			else
			{
				if (string.IsNullOrEmpty(options.MetadataAddress) && string.IsNullOrEmpty(options.Authority))
				{
					return;
				}
				if (string.IsNullOrEmpty(options.MetadataAddress) && !string.IsNullOrEmpty(options.Authority))
				{
					options.MetadataAddress = options.Authority;
					if (!options.MetadataAddress.EndsWith("/", StringComparison.Ordinal))
					{
						options.MetadataAddress += "/";
					}
					options.MetadataAddress += ".well-known/openid-configuration";
				}
				if (options.RequireHttpsMetadata && !options.MetadataAddress.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
				{
					throw new InvalidOperationException("The MetadataAddress or Authority must use HTTPS unless disabled for development by setting RequireHttpsMetadata=false.");
				}
				HttpClient httpClient = new HttpClient(options.BackchannelHttpHandler ?? new HttpClientHandler());
				httpClient.Timeout = options.BackchannelTimeout;
				httpClient.MaxResponseContentBufferSize = 10485760L;
				options.ConfigurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(options.MetadataAddress, new OpenIdConnectConfigurationRetriever(), new HttpDocumentRetriever(httpClient)
				{
					RequireHttps = options.RequireHttpsMetadata
				});
			}
		}
	}
}
