﻿using Application.Exceptions;
using Application.Extensions;
using Application.Interfaces.Contexts;
using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.Users;
using Application.Interfaces.Services;
using Application.Interfaces.Services.ExportImport;
using Application.Interfaces.Services.Identity;
using Application.Interfaces.Services.Mailing;
using Azure.Storage.Blobs;
using Domain.Entities.Users;
using Domain.Enums;
using Domain.Enums.Memberships;
using Hangfire;
using Infrastructure.Authentication;
using Infrastructure.Configurations;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Persistence.Repositories.Users;
using Infrastructure.Services;
using Infrastructure.Services.Caching;
using Infrastructure.Services.ExportImport;
using Infrastructure.Services.Identity;
using Infrastructure.Services.Mailing;
using Infrastructure.Services.Storage;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using SendGrid.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;
using SameSiteMode = Microsoft.AspNetCore.Http.SameSiteMode;

namespace Infrastructure.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
        {
            // Serializer
            services.AddSerializer();

            // Persistence
            services.AddDbContext(configuration);
            services.AddRepositories();

            // Identity
            services.AddIdentity(configuration);

            // Authentication
            services.AddAuthentication(configuration);

            // Authorization
            services.AddAuthorization();

            // Mailing
            services.AddMailing(configuration);

            // Storage
            services.AddAzureStorage(configuration);

            // BackgroundJobs
            services.AddBackgroundJobs(configuration);

            // CORS
            services.AddCorsPolicy(configuration);

            // Antiforgery
            services.AddAntiforgery();

            // MVC
            services.AddControllersWithViews()
                .AddMicrosoftIdentityUI();
            services.AddRazorPages();

            // Caching
            services.AddCaching(configuration);

            // Real-Time Services
            services.AddChatHub(configuration);

            // ExportImport
            services.AddExportImport();
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
                .AddTransient<IPortalConfigRepository, PortalConfigRepository>()
                .AddTransient<IUserProfileImageRepository, UserProfileImageRepository>()
                .AddTransient<IUserRepository, UserRepository>();
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

            // TODO: fix this to catter for basic auth.... cause GraphGroups/DisplayNames needs it
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
                }, jwtBearerScheme: Constants.AzureAdB2C)
                ;

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
                // Handling SameSite cookie according to https://docs.microsoft.com/en-us/aspnet/core/security/samesite?view=aspnetcore-3.1
                options.HandleSameSiteCookieCompatibility();
            });

            services.Configure<OpenIdConnectOptions>(configuration.GetSection("AzureAdB2CApi"));

            services.AddMicrosoftIdentityWebAppAuthentication(configuration, "AzureAdB2CApi");
        }

        private static void AddBasicAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<BasicAuthenticationConfig>(configuration.GetSection("BasicAuthentication"));

            services.AddAuthentication()
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("Basic", null);
        }

        #endregion

        #region Authorization

        private static void AddAuthorization(this IServiceCollection services)
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

                opts.AddPolicy("Hangfire", builder =>
                {
                    builder
                        .AddAuthenticationSchemes(OpenIdConnectDefaults.AuthenticationScheme)
                        .RequireRole(Role.Administrator.ToString())
                        .RequireAuthenticatedUser();
                });
            });

            services.AddRequiredScopeAuthorization();
        }

        #endregion

        #region Mailing

        private static void AddMailing(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MailConfig>(configuration.GetSection("Mail"));

            // TODO: logic to select SMTP service
            services.AddScoped<IMailService, GoogleMailService>();
            //services.AddSendGridMailService(configuration);


            services.AddTransient<IEmailTemplateService, EmailTemplateService>();
        }

        private static void AddSendGridMailService(this IServiceCollection services, IConfiguration configuration)
        {
            var mailConfig = configuration.GetSection("Mail").Get<MailConfig>() ?? new();

            services.AddSendGrid(options =>
            {
                options.ApiKey = mailConfig.SendGridSMTP.ApiKey;
            });

            services.AddScoped<IMailService, SendGridMailService>();
        }

        #endregion

        #region Storage

        private static void AddAzureStorage(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AzureStorageConfig>(configuration.GetSection("AzureStorage"));

            var azureStorageConfig = configuration.GetSection("AzureStorage").Get<AzureStorageConfig>() ?? new();

            services.AddScoped(x =>
            {
                try
                {
                    var blobServiceClient = new BlobServiceClient(azureStorageConfig.ConnectionString);

                    return blobServiceClient;
                }
                catch (Exception ex)
                {
                    throw new RestException(HttpStatusCode.InternalServerError, ex.Message);
                }
            });

            services.AddScoped<IDataFileStorageService, DataFileStorageService>();
        }

        #endregion

        #region Identity

        private static void AddIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ICurrentUserService, CurrentUserService>();
            services.AddTransient<IUserService, UserService>();
            services.AddGraph(configuration);
        }

        private static void AddGraph(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AzureAdB2CManagementConfig>(configuration.GetSection("AzureAdB2CManagement"));
            services.Configure<GraphConfig>(configuration.GetSection("Graph"));

            services.AddTransient<IGraphService, GraphService>();
        }

        #endregion

        #region Misc

        private static void AddBackgroundJobs(this IServiceCollection services, IConfiguration configuration)
        {
            // Add Hangfire services.
            services.AddHangfire(x => x
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(configuration.GetConnectionString("Primary")));

            // Add the processing server as IHostedService
            services.AddHangfireServer();
        }

        private static void AddCorsPolicy(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CORSConfig>(configuration.GetSection("CORS"));

            var cORSConfig = configuration.GetSection("CORS").Get<CORSConfig>() ?? new();
            var origins = new List<string>();

            if (cORSConfig.Blazor.Any())
                origins.AddRange(cORSConfig.Blazor);

            services.AddCors(opt =>
                opt.AddPolicy("CORS", policy =>
                    policy.AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .WithOrigins(origins.ToArray())));
        }

        private static void AddAntiforgery(this IServiceCollection services)
        {
            services.AddAntiforgery(options =>
            {
                options.SuppressXFrameOptionsHeader = true;
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            });
        }

        private static void AddSerializer(this IServiceCollection services)
        {
            services.AddTransient<ISerializerService, NewtonSoftService>();
        }

        private static void AddCaching(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CacheConfig>(configuration.GetSection("Cache"));

            var cacheConfig = configuration.GetSection("Cache").Get<CacheConfig>() ?? new();

            switch (cacheConfig.DistributedCacheType)
            {
                case DistributedCacheType.Redis:
                    {
                        services.AddStackExchangeRedisCache(options =>
                        {
                            options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions()
                            {
                                AbortOnConnectFail = true,
                                EndPoints = { cacheConfig.Redis.RedisURL }
                            };
                        });

                        services.AddTransient<ICacheService, DistributedCacheService>();

                        break;
                    }
                case DistributedCacheType.SqlServer:
                    {
                        services.AddDistributedSqlServerCache(options =>
                        {
                            options.ConnectionString = configuration.GetConnectionString("Primary");
                            options.SchemaName = "dbo";
                            options.TableName = "CacheItems";
                        });

                        services.AddTransient<ICacheService, DistributedCacheService>();

                        break;
                    }
                case DistributedCacheType.InMemory:
                    {
                        services.AddDistributedMemoryCache();

                        services.AddTransient<ICacheService, DistributedCacheService>();

                        break;
                    }
                case DistributedCacheType.None:
                default:
                    {
                        services.AddMemoryCache();

                        services.AddTransient<ICacheService, LocalCacheService>();

                        break;
                    }
            };
        }

        private static void AddChatHub(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SignalRConfig>(configuration.GetSection("SignalR"));

            var signalRConfig = configuration.GetSection("SignalR").Get<SignalRConfig>() ?? new();

            switch (signalRConfig.SignalRType)
            {
                case SignalRType.Azure:
                    {
                        services.AddSignalR().AddAzureSignalR(options =>
                        {
                            options.ConnectionString = signalRConfig.Azure.ConnectionString;
                        });

                        break;
                    }
                case SignalRType.None:
                default:
                    {
                        services.AddSignalR();

                        break;
                    }
            };
        }

        private static void AddExportImport(this IServiceCollection services)
        {
            services.AddTransient<IExportManager, ExportManager>();
            services.AddTransient<IImportManager, ImportManager>();
        }

        #endregion
    }
}
