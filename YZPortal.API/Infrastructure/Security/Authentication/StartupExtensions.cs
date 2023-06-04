using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Identity.Web;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;
using YZPortal.API.Infrastructure.Swagger;
using YZPortal.API.Infrastructure.Security.AzureAd;
using YZPortal.API.Infrastructure.Security.AzureAdB2C;
using YZPortal.API.Infrastructure.Security.Jwt;
using Microsoft.AspNetCore.Authentication;
using YZPortal.API.Infrastructure.Security.Authentication.BasicAuthentication;

namespace YZPortal.API.Infrastructure.Security.Authentication
{
    public static class StartupExtensions
    {
        public static void AddAuthentications(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtOptions = configuration.GetSection("Jwt").Get<JwtIssuerOptions>();
            var swaggerOptions = configuration.GetSection("Swagger").Get<SwaggerOptions>() ?? new();
            var azureAdApiOptions = configuration.GetSection("AzureAdApi").Get<AzureAdApiOptions>() ?? new();
            var azureAdB2CApiOptions = configuration.GetSection("AzureAdB2CApi").Get<AzureAdB2CApiOptions>() ?? new();

            // Jwt Bearer
            services.AddAuthentication()
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, x =>
            {
                x.SaveToken = true;
                x.RequireHttpsMetadata = false;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = jwtOptions.SigningCredentials.Key,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience ?? jwtOptions.Issuer,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                };
            });

            // Basic
            services.AddAuthentication()
            .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("Basic", null);

            // Azure Ad
            services.AddAuthentication()
            .AddMicrosoftIdentityWebApi(x =>
            {
                configuration.Bind("AzureAdApi", x);
                x.TokenValidationParameters.NameClaimType = "name";
            }, x =>
            {
                configuration.Bind("AzureAdApi", x);
            }, jwtBearerScheme: Constants.AzureAd);

            // Azure Ad B2C
            services.AddAuthentication()
            .AddMicrosoftIdentityWebApi(x =>
            {
                configuration.Bind("AzureAdB2CApi", x);
                x.TokenValidationParameters.NameClaimType = "name";
            }, x =>
            {
                configuration.Bind("AzureAdB2CApi", x);
            }, jwtBearerScheme: Constants.AzureAdB2C);

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Azures";
            })
            .AddPolicyScheme("Azures", "Authorize AzureAd or AzureAdBearer", options =>
            {
                options.ForwardDefaultSelector = context =>
                {
                    var authorization = context.Request.Headers[HeaderNames.Authorization].ToString();
                    var token = authorization.Substring("Bearer ".Length).Trim();
                    var jwtHandler = new JwtSecurityTokenHandler();
                    var issuer = jwtHandler.ReadJwtToken(token).Claims.FirstOrDefault(x => x.Type == "aud")?.Value;
                    var azureAdApiOptionsAudience = azureAdApiOptions?.Audience ?? "";
                    var azureAdB2CApiOptionsClientId = azureAdB2CApiOptions?.ClientId ?? "";

                    if (string.Equals(issuer, azureAdApiOptionsAudience, StringComparison.OrdinalIgnoreCase))
                    {
                        if (!string.IsNullOrEmpty(azureAdApiOptionsAudience))
                            return Constants.AzureAd;
                    }
                    else if (string.Equals(issuer, azureAdB2CApiOptionsClientId, StringComparison.OrdinalIgnoreCase))
                    {
                        if (!string.IsNullOrEmpty(azureAdB2CApiOptionsClientId))
                            return Constants.AzureAdB2C;
                    }

                    return JwtBearerDefaults.AuthenticationScheme;
                };
            });

            // Remove default behaviour of MVC redirecting to login page
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = string.Empty;
                options.AccessDeniedPath = string.Empty;

                // Not creating a new object since ASP.NET Identity has created
                // one already and hooked to the OnValidatePrincipal event.
                // See https://github.com/aspnet/AspNetCore/blob/5a64688d8e192cacffda9440e8725c1ed41a30cf/src/Identity/src/Identity/IdentityServiceCollectionExtensions.cs#L56
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                };
            });
        }
    }
}
