using Infrastructure.Configurations;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using YZPortal.API.Extensions;

namespace YZPortal.API.Extensions
{
    internal static class IApplicationBuilderExtensions
    {
        internal static IApplicationBuilder UseSwagger(this IApplicationBuilder app, IApiVersionDescriptionProvider provider, IConfiguration configuration)
        {
            var azureAdB2CSwaggerOptions = configuration.GetSection("AzureAdB2CSwagger").Get<AzureAdB2CSwaggerConfig>() ?? new();

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
                opts.OAuthClientId(azureAdB2CSwaggerOptions.ClientId);
                opts.OAuthClientSecret(azureAdB2CSwaggerOptions.ClientId);
                opts.OAuthAppName(azureAdB2CSwaggerOptions?.AppName ?? "");
                opts.OAuthUseBasicAuthenticationWithAccessCodeGrant();
            });

            return app;
        }
    }
}
