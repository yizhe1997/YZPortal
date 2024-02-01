using Hangfire.Dashboard;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Application.Extensions;
using Domain.Enums.Memberships;
using Infrastructure.Configurations;

namespace Infrastructure.Services.BackgroundJob
{
    public class DashboardOpenIdAuthorizationFilter : IDashboardAuthorizationFilter
    {
        private ConfigurationManager<OpenIdConnectConfiguration> _configManager;
        private TokenValidationParameters? _jwtValidationParameters;
        private JwtSecurityTokenHandler _jwtSecurityTokenHandler = new();
        private readonly AzureAdB2CManagementConfig _azureAdB2CManagementConfig;

        public DashboardOpenIdAuthorizationFilter(AzureAdB2CManagementConfig azureAdB2CManagementConfig)
        {
            _azureAdB2CManagementConfig = azureAdB2CManagementConfig;
            _configManager = new ConfigurationManager<OpenIdConnectConfiguration>(_azureAdB2CManagementConfig.OpenIdConfigUrl, new OpenIdConnectConfigurationRetriever());
        }

        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            var access_token = httpContext.Request.Cookies["HangFireCookie"];

            if (string.IsNullOrEmpty(access_token))
            {
                return false;
            }

            try
            {
                if (!_configManager.IsLastKnownGoodValid)
                {
                    OpenIdConnectConfiguration config = _configManager.GetConfigurationAsync().Result;
                    _jwtValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        ValidIssuers = new string[] { config.Issuer, _azureAdB2CManagementConfig.ValidIssuer },
                        ConfigurationManager = _configManager
                    };
                }

                var claimsPrincipal = _jwtSecurityTokenHandler.ValidateToken(access_token, _jwtValidationParameters, out var validatedToken);

                var roleClaim = claimsPrincipal.GetRoleClaim().ToList();

                if (!roleClaim.Contains(Role.Administrator.ToString()))
                {
                    return false;
                }
            }
            catch
            {
                throw;
            }

            return true;
        }
    }
}
