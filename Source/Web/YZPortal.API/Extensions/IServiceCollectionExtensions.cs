using Infrastructure.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.Reflection;
using YZPortal.API.Filters;

namespace YZPortal.API.Extensions
{
    internal static class IServiceCollectionExtensions
    {
        internal static void AddWebLayer(this IServiceCollection services, IConfiguration configuration)
        {
            // TODO: declutter?
            // Add cross-origin resource sharing to IServiceCollection
            services.AddCors();

            services.AddMvc(opts =>
            {
                opts.Filters.Add(typeof(ValidatorActionFilter));
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressConsumesConstraintForFormFileParameters = true;
                options.SuppressInferBindingSourcesForParameters = true;
                options.SuppressModelStateInvalidFilter = true;
                options.SuppressMapClientErrors = true;
            })
            .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            // Max Value Allowed Content Length
            services.Configure<KestrelServerOptions>(options =>
            {
                // Increase for file size limit. if don't set default value is: 30 MB
                options.Limits.MaxRequestBodySize = int.MaxValue;
            });

            services.AddAPIVersioning();
            services.AddSwagger(configuration);
        }

        internal static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();

            services.Configure<AzureAdB2CSwaggerConfig>(configuration.GetSection("AzureAdB2CSwagger"));

            services.AddSwaggerGen(opts =>
            {
                var azureAdB2CApiOptions = configuration.GetSection("AzureAdB2CApi").Get<AzureAdB2CApiConfig>() ?? new();
                var azureAdB2CSwaggerOptions = configuration.GetSection("AzureAdB2CSwagger").Get<AzureAdB2CSwaggerConfig>() ?? new();

                var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

                foreach (var description in provider.ApiVersionDescriptions)
                {
                    var assembly = Assembly.GetAssembly(typeof(Program));

                    opts.SwaggerDoc(description.GroupName, new OpenApiInfo
                    {
                        Title = $"YZ Portal API {description.ApiVersion}",
                        Description = description.IsDeprecated ? $"YZPortal API {description.ApiVersion} - DEPRECATED" : $"YZ Portal API",
                        Version = description.ApiVersion.ToString()
                    });
                    opts.DocInclusionPredicate((version, apiDescription) => true);
                }

                #region Add Security Definition

                opts.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                opts.AddSecurityDefinition(Constants.AzureAdB2C, new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Scheme = Constants.AzureAdB2C,
                    Description = "Azure AD B2C authorization",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Flows = new OpenApiOAuthFlows()
                    {
                        Implicit = new OpenApiOAuthFlow()
                        {
                            AuthorizationUrl = new Uri($"{azureAdB2CApiOptions.Instance}/{azureAdB2CApiOptions.Domain}/oauth2/v2.0/authorize?p={azureAdB2CApiOptions.SignUpSignInPolicyId}"),
                            TokenUrl = new Uri($"{azureAdB2CApiOptions.Instance}/{azureAdB2CApiOptions.Domain}/oauth2/v2.0/token?p={azureAdB2CApiOptions.SignUpSignInPolicyId}"),
                            Scopes = new Dictionary<string, string>
                                {
                                    {
                                        azureAdB2CSwaggerOptions.Scope,
                                        "Provides authorization token for Dealer Portal WebAPI"
                                    }
                                }
                        }
                    }
                });

                #endregion

                #region Add Security Requirements

                var openApiSecurityRequirement = new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            },
                            Scheme = JwtBearerDefaults.AuthenticationScheme,
                            Name = "Authorization",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    },
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = Constants.AzureAdB2C
                            },
                            Scheme = Constants.AzureAdB2C,
                            Name = "Authorization",
                            In = ParameterLocation.Header,
                        },
                        new List<string>() { azureAdB2CSwaggerOptions.Scope }
                    }
                };

                opts.AddSecurityRequirement(openApiSecurityRequirement);

                #endregion

                opts.CustomSchemaIds(s => s.FullName?.Replace("+", "."));
                opts.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                opts.DescribeAllParametersInCamelCase();

                opts.OperationFilter<SecurityRequirementsOperationFilter>(configuration);

                #region Swagger XML documentation file

                // Ref: https://learn.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-7.0&tabs=visual-studio
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                opts.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

                #endregion
            });

            return services;
        }

        internal static IServiceCollection AddAPIVersioning(this IServiceCollection services)
        {
            // Add API versioning
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            });

            // Add API version exploration
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VV";
                options.SubstituteApiVersionInUrl = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            });

            return services;
        }
    }
}
