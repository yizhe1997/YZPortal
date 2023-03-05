using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using YZPortal.API.Infrastructure.Security.AzureAd;
using YZPortal.API.Infrastructure.Security.AzureAdB2C;

namespace YZPortal.API.Infrastructure.Swagger
{
    public static class StartupExtensions
    {
        public static void AddSwaggerOption(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SwaggerOptions>(configuration.GetSection("Swagger"));
        }

        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services, IConfiguration configuration)
        {
            //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            services.AddSwaggerGen(opts =>
            {
                var swaggerOptions = configuration.GetSection("Swagger").Get<SwaggerOptions>() ?? new();

                var azureAdApiOptions = configuration.GetSection("AzureAdApi").Get<AzureAdApiOptions>() ?? new();
                var azureAdSwaggerOptions = configuration.GetSection("AzureAdSwagger").Get<AzureAdSwaggerOptions>() ?? new();

                var azureAdB2CApiOptions = configuration.GetSection("AzureAdB2CApi").Get<AzureAdB2CApiOptions>() ?? new();
                var azureAdB2CSwaggerOptions = configuration.GetSection("AzureAdB2CSwagger").Get<AzureAdB2CSwaggerOptions>() ?? new();

                var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

                foreach (var description in provider.ApiVersionDescriptions)
                {
                    var assembly = Assembly.GetAssembly(typeof(Program));

                    opts.SwaggerDoc(description.GroupName, new OpenApiInfo
                    {
                        Title = $"A365 DealerPortal API {description.ApiVersion}",
                        Description = description.IsDeprecated ? $"YZPortal API {description.ApiVersion} - DEPRECATED" : $"A365 DealerPortal API",
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

                if (swaggerOptions.IsAzureAdOAuth2Provider == true)
                {
                    opts.AddSecurityDefinition(Constants.AzureAd, new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.OAuth2,
                        Scheme = Constants.AzureAd,
                        Description = "Azure AD authorization",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Flows = new OpenApiOAuthFlows()
                        {
                            Implicit = new OpenApiOAuthFlow()
                            {
                                AuthorizationUrl = new Uri($"{azureAdApiOptions.Instance}/{azureAdApiOptions.TenantId}/oauth2/v2.0/authorize"),
                                TokenUrl = new Uri($"{azureAdApiOptions.Instance}/{azureAdApiOptions.TenantId}/oauth2/v2.0/token"),
                                Scopes = new Dictionary<string, string>
                                {
                                    {
                                        azureAdSwaggerOptions.Scope,
                                        "Provides authorization token for YZPortal API"
                                    }
                                }
                            }
                        }
                    });
                }
                else
                {
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
                }

				#endregion

				#region Add Security Requirements

				var openApiSecurityRequirement = new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme {
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
                    }
                };

                if (swaggerOptions.IsAzureAdOAuth2Provider == true)
                {
                    openApiSecurityRequirement.Add(new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = Constants.AzureAd
                        },
                        Scheme = Constants.AzureAd,
                        Name = "Authorization",
                        In = ParameterLocation.Header
                    },
                    new List<string>() { azureAdSwaggerOptions.Scope });
                }
                else
                {
                    openApiSecurityRequirement.Add(new OpenApiSecurityScheme
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
                    new List<string>() { azureAdB2CSwaggerOptions.Scope });
                }

                opts.AddSecurityRequirement(openApiSecurityRequirement);

				#endregion

				opts.CustomSchemaIds(s => s.FullName.Replace("+", "."));
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

        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app, IApiVersionDescriptionProvider provider, IConfiguration configuration)
        {
            var swaggerOptions = configuration.GetSection("Swagger").Get<SwaggerOptions>() ?? new();
            var azureAdB2CSwaggerOptions = configuration.GetSection("AzureAdB2CSwagger").Get<AzureAdB2CSwaggerOptions>() ?? new();
            var azureAdSwaggerOptions = configuration.GetSection("AzureAdSwagger").Get<AzureAdSwaggerOptions>() ?? new();

            app.UseSwagger(opts =>
            {
                opts.RouteTemplate = "docs/{documentName}/docs.json";
                //opts.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                //{
                //    swaggerDoc.Host = httpReq.Host.Value;
                //    swaggerDoc.Schemes = new List<string>() { httpReq.Scheme };
                //    swaggerDoc.BasePath = httpReq.PathBase;
                //});
            });

            app.UseSwaggerUI(opts =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    opts.SwaggerEndpoint($"/docs/{description.GroupName}/docs.json", description.GroupName.ToUpperInvariant());
                }

                opts.RoutePrefix = "docs";
                opts.DefaultModelsExpandDepth(-1);
                opts.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
                opts.OAuthClientId(swaggerOptions.IsAzureAdOAuth2Provider ? azureAdSwaggerOptions.ClientId : azureAdB2CSwaggerOptions.ClientId);
                opts.OAuthClientSecret(swaggerOptions.IsAzureAdOAuth2Provider ? azureAdSwaggerOptions.ClientId : azureAdB2CSwaggerOptions.ClientId);
                opts.OAuthAppName(azureAdB2CSwaggerOptions?.AppName ?? "");
                opts.OAuthUseBasicAuthenticationWithAccessCodeGrant();
            });

            return app;
        }

        public class SecurityRequirementsOperationFilter : IOperationFilter
        {
            public IConfiguration Configuration { get; }

            public SecurityRequirementsOperationFilter(IConfiguration configuration)
            {
                Configuration = configuration;
            }

            public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {
                var azureAdSwaggerOptions = Configuration.GetSection("AzureAdSwagger").Get<AzureAdSwaggerOptions>() ?? new();
                var azureAdB2CSwaggerOptions = Configuration.GetSection("AzureAdB2CSwagger").Get<AzureAdB2CSwaggerOptions>() ?? new();

                if (context != null && operation != null)
                {
                    // AuthenticationSchemes map to scopes
                    // for class level authentication schemes
                    var requiredScopes = context.MethodInfo.DeclaringType?
                            .GetCustomAttributes(true)
                            .OfType<AuthorizeAttribute>()
                            .Select(attr => attr.AuthenticationSchemes)
                            .Distinct();

                    //  for method level authentication scheme
                    var requiredScopes2 = context.MethodInfo
                            .GetCustomAttributes(true)
                            .OfType<AuthorizeAttribute>()
                            .Select(attr => attr.AuthenticationSchemes)
                            .Distinct();

                    bool requireAuth = false;
                    string id = "";
                    var scope = "";

                    if (requiredScopes.Contains(Constants.AzureAdB2C) || requiredScopes2.Contains(Constants.AzureAdB2C))
                    {
                        requireAuth = true;
                        id = Constants.AzureAdB2C;
                        scope = azureAdB2CSwaggerOptions?.Scope ?? "";
                    }
                    else if (requiredScopes.Contains(JwtBearerDefaults.AuthenticationScheme) || requiredScopes2.Contains(JwtBearerDefaults.AuthenticationScheme))
                    {
                        requireAuth = true;
                        id = JwtBearerDefaults.AuthenticationScheme;
                    }
                    else if (requiredScopes.Contains(Constants.AzureAd) || requiredScopes2.Contains(Constants.AzureAd))
                    {
                        requireAuth = true;
                        id = Constants.AzureAd;
                        scope = azureAdSwaggerOptions?.Scope ?? "";
                    }

                    if (requireAuth && !string.IsNullOrEmpty(id))
                    {
                        operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });

                        operation.Security = new List<OpenApiSecurityRequirement>
                        {
                            new OpenApiSecurityRequirement
                            {
                                {
                                    new OpenApiSecurityScheme
                                    {
                                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = id },
                                        Name = "Authorization",
                                        Scheme = id,
                                        In = ParameterLocation.Header
                                    },
                                    new List<string>() { scope }
                                }
                            }
                        };
                    }
                }
            }
        }
    }
}
