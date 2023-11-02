using Infrastructure.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace YZPortal.API.Filters
{
    internal class SecurityRequirementsOperationFilter : IOperationFilter
    {
        public IConfiguration Configuration { get; }

        public SecurityRequirementsOperationFilter(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var azureAdB2CSwaggerOptions = Configuration.GetSection("AzureAdB2CSwagger").Get<AzureAdB2CSwaggerConfig>() ?? new();

            if (context != null && operation != null)
            {
                // AuthenticationSchemes map to scopes
                // for class level authentication schemes
                var requiredScopes = context.MethodInfo.DeclaringType?
                        .GetCustomAttributes(true)
                        .OfType<AuthorizeAttribute>()
                        .Select(attr => attr.AuthenticationSchemes ?? JwtBearerDefaults.AuthenticationScheme)
                        .Distinct() ?? new List<string>();

                //  for method level authentication scheme
                var requiredScopes2 = context.MethodInfo
                        .GetCustomAttributes(true)
                        .OfType<AuthorizeAttribute>()
                        .Select(attr => attr.AuthenticationSchemes ?? JwtBearerDefaults.AuthenticationScheme)
                        .Distinct() ?? new List<string>();

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
