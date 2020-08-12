using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Application.AuthApi.Middleware
{
    /// <summary>
    /// source:System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler
    /// </summary>
    public class CodeJwtSecurityTokenHandler : JwtSecurityTokenHandler
    {
        protected override void ValidateAudience(IEnumerable<string> audiences, JwtSecurityToken jwtToken, TokenValidationParameters validationParameters)
        {
            ValidateAudience(audiences, jwtToken, validationParameters);
        }

		public static void ValidateAudience(IEnumerable<string> audiences, SecurityToken securityToken, TokenValidationParameters validationParameters)
		{
			if (validationParameters == null)
			{
				throw LogHelper.LogArgumentNullException("validationParameters");
			}
			if (!validationParameters.ValidateAudience)
			{
				LogHelper.LogWarning("IDX10233: ValidateAudience property on ValidationParameters is set to false. Exiting without validating the audience.");
				return;
			}
			if (validationParameters.AudienceValidator != null)
			{
				if (!validationParameters.AudienceValidator(audiences, securityToken, validationParameters))
				{
					throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidAudienceException(LogHelper.FormatInvariant("IDX10231: Audience validation failed. Delegate returned false, securitytoken: '{0}'.", securityToken))
					{
						InvalidAudience = SerializeAsSingleCommaDelimitedString(audiences)
					});
				}
				return;
			}
			if (audiences == null)
			{
				throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidAudienceException("IDX10207: Unable to validate audience. The 'audiences' parameter is null.")
				{
					InvalidAudience = null
				});
			}
			if (string.IsNullOrWhiteSpace(validationParameters.ValidAudience) && validationParameters.ValidAudiences == null)
			{
				throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidAudienceException("IDX10208: Unable to validate audience. validationParameters.ValidAudience is null or whitespace and validationParameters.ValidAudiences is null.")
				{
					InvalidAudience = SerializeAsSingleCommaDelimitedString(audiences)
				});
			}
			foreach (string audience in audiences)
			{
				if (string.IsNullOrWhiteSpace(audience))
				{
					continue;
				}
				if (validationParameters.ValidAudiences != null)
				{
					foreach (string validAudience in validationParameters.ValidAudiences)
					{
						if (string.Equals(audience, validAudience, StringComparison.Ordinal))
						{
							LogHelper.LogInformation("IDX10234: Audience Validated.Audience: '{0}'", audience);
							return;
						}
					}
				}
				if (!string.IsNullOrWhiteSpace(validationParameters.ValidAudience) && string.Equals(audience, validationParameters.ValidAudience, StringComparison.Ordinal))
				{
					LogHelper.LogInformation("IDX10234: Audience Validated.Audience: '{0}'", audience);
					return;
				}
			}
			throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidAudienceException(LogHelper.FormatInvariant("IDX10214: Audience validation failed. Audiences: '{0}'. Did not match: validationParameters.ValidAudience: '{1}' or validationParameters.ValidAudiences: '{2}'.", SerializeAsSingleCommaDelimitedString(audiences), validationParameters.ValidAudience ?? "null", SerializeAsSingleCommaDelimitedString(validationParameters.ValidAudiences)))
			{
				InvalidAudience = SerializeAsSingleCommaDelimitedString(audiences)
			});
		}

		internal static string SerializeAsSingleCommaDelimitedString(IEnumerable<string> strings)
		{
			if (strings == null)
			{
				return "null";
			}
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			foreach (string @string in strings)
			{
				if (flag)
				{
					stringBuilder.AppendFormat("{0}", @string ?? "null");
					flag = false;
				}
				else
				{
					stringBuilder.AppendFormat(", {0}", @string ?? "null");
				}
			}
			if (flag)
			{
				return "empty";
			}
			return stringBuilder.ToString();
		}


	}
}
