using Application.Extensions;
using Application.Interfaces.Contexts;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Interfaces.Services.Identity;
using Domain.Entities.Users;
using Infrastructure.Authentication;
using Infrastructure.Configurations;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Services;
using Infrastructure.Services.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Infrastructure.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
        {
            // Persistence
            services.AddDbContext(configuration);
            services.AddRepositories();

            // General
            services.AddTransient<ICurrentUserService, CurrentUserService>();
            services.AddTransient<IUserService, UserService>();
            services.AddGraph(configuration);

            // Authentication
            services.AddAuthentication(configuration);

            // Authorization
            services.AddAuthorization();
        }

        #region Persistence

        private static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            // Db Context
            var conn = configuration.GetConnectionString("Primary");
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(conn);
                options.EnableSensitiveDataLogging(true);
            });

            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

            // Prereq Microsoft.AspNetCore.Identity and to use usermanager (not meant for ef core migration)
            services.AddIdentity<User, IdentityRole<Guid>>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
        }

        private static void AddRepositories(this IServiceCollection services)
        {
            services
                .AddTransient(typeof(IGenericRepository<,>), typeof(GenericRepository<,>))
                .AddTransient(typeof(IUnitOfWork<>), typeof(UnitOfWork<>))
                .AddTransient<IPortalConfigRepository, PortalConfigRepository>();
        }

        #endregion

        #region Authentication

        private static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var azureAdB2CApiOptions = configuration.GetSection("AzureAdB2CApi").Get<AzureAdB2CApiConfig>() ?? new();

            // Add Authentication types
            services.AddJwt(configuration);
            services.AddAzureAdB2C(configuration);
            services.AddBasicAuth(configuration);
            
            // Policy Scheme
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
                    var azureAdB2CApiOptionsClientId = azureAdB2CApiOptions?.ClientId ?? "";

                    if (string.Equals(issuer, azureAdB2CApiOptionsClientId, StringComparison.OrdinalIgnoreCase))
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

        private static void AddJwt(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtOptions = configuration.GetSection("Jwt").Get<JwtIssuerConfig>();

            services.Configure<JwtIssuerConfig>(configuration.GetSection("Jwt"));

            services.AddTransient<IJwtService, JwtService>();

            services.AddAuthentication()
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, x =>
            {
                x.SaveToken = true;
                x.RequireHttpsMetadata = false;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey ?? string.Empty)), SecurityAlgorithms.HmacSha256).Key,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience ?? jwtOptions.Issuer,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                };
            });
        }

        private static void AddAzureAdB2C(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AzureAdB2CApiConfig>(configuration.GetSection("AzureAdB2CApi"));

            services.AddAuthentication()
            .AddMicrosoftIdentityWebApi(x =>
            {
                configuration.Bind("AzureAdB2CApi", x);
                x.TokenValidationParameters.NameClaimType = "name";
            }, x =>
            {
                configuration.Bind("AzureAdB2CApi", x);
            }, jwtBearerScheme: Constants.AzureAdB2C);
        }

        private static void AddBasicAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<BasicAuthenticationConfig>(configuration.GetSection("BasicAuthentication"));

            services.AddAuthentication()
            .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("Basic", null);
        }

        #endregion

        #region Authorization

        public static void AddAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(opts =>
            {
                // Role
                opts.AddAdministratorPolicy();
                opts.AddGeneralPolicy();

                // Subscription
                opts.AddBadmintonSubscriptionPolicy();

                // Authentication Schema
                var allAuthenSchemes = new AuthorizationPolicyBuilder(
                        JwtBearerDefaults.AuthenticationScheme,
                        Constants.AzureAdB2C);
                opts.AddPolicy("AllAuthenSchemes", allAuthenSchemes
                    .RequireAuthenticatedUser()
                    .Build());
            });
            services.AddRequiredScopeAuthorization();
        }

        #endregion

        #region Misc

        private static void AddGraph(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AzureAdB2CManagementConfig>(configuration.GetSection("AzureAdB2CManagement"));
            services.Configure<GraphConfig>(configuration.GetSection("Graph"));

            services.AddTransient<IGraphService, GraphService>();
        }

        #endregion
    }
}
