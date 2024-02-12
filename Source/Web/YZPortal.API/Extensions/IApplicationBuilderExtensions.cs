using Application.Exceptions;
using Application.Models;
using Infrastructure.Configurations;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Graph.Models.ODataErrors;
using Serilog;
using System.Net;
using YZPortal.API.Extensions;

namespace YZPortal.API.Extensions
{
    internal static class IApplicationBuilderExtensions
    {
        internal static IApplicationBuilder UseInfrastructure(this IApplicationBuilder builder, IConfiguration configuration) =>
            builder
                .UseMiddlewareExceptionHandler()
                .UseSerilogRequestLogging()
                .UseCorsPolicy()
                .UseHsts()
                .UseHttpsRedirection()
                .UseStaticFiles()
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .UseRequestLocalization(options =>
                {
                    var supportedCultures = new[] { "en", "de" };
                    options.SetDefaultCulture(supportedCultures[0])
                        .AddSupportedCultures(supportedCultures)
                        .AddSupportedUICultures(supportedCultures);
                })
                .UseHangfire(configuration);

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

        internal static IApplicationBuilder UseMiddlewareExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    var methodPathString = context.Request.Method + context.Request.Path;

                    if (contextFeature != null)
                    {
                        var serviceProvider = new ServiceCollection()
                          .AddLogging(cfg => cfg.AddConsole())
                          .Configure<LoggerFilterOptions>(cfg => cfg.MinLevel = LogLevel.Information)
                          .BuildServiceProvider();

                        var logger = serviceProvider.GetService<ILogger<Exception>>();

                        var innerException = contextFeature.Error.InnerException;

                        // RestException
                        if (typeof(RestException) == contextFeature.Error.GetType() || typeof(RestException) == innerException?.GetType())
                        {
                            var ex = innerException == null ? (RestException)contextFeature.Error : (RestException)innerException;
                            context.Response.StatusCode = ex.Code == HttpStatusCode.NotFound || ex.Code == HttpStatusCode.NoContent ?
                                context.Response.StatusCode :
                                (int)ex.Code;

                            await context.Response.WriteAsync(new Result()
                            {
                                Messages = new List<string>() { ex.Error }
                            }.ToJSONString());

                            logger.LogError($"RestException thrown for method {methodPathString}.\r\nStatusCode:\r\n{(int)ex.Code}\r\nError message:\r\n{ex.Error}");
                        }
                        // ODataError
                        else if (typeof(ODataError) == contextFeature.Error.GetType() || typeof(ODataError) == innerException?.GetType())
                        {
                            var ex = innerException == null ? (ODataError)contextFeature.Error : (ODataError)innerException;
                            // TODO: this is not working it always turns to status code 0, try adding use to group again for example...
                            var statusCode = !Enum.TryParse(ex.Error?.Code, out HttpStatusCode httpStatusCode) ? (int)httpStatusCode : context.Response.StatusCode;
                            var errMsg = ex.Error?.Message ?? context.Response.StatusCode.ToString();

                            await context.Response.WriteAsync(new Result()
                            {
                                Messages = new List<string>() { errMsg }
                            }.ToJSONString());

                            logger.LogError($"ODataError thrown for method {methodPathString}.\r\nStatusCode:\r\n{statusCode}\r\nError message:\r\n{errMsg}");
                        }
                        else
                        {
                            await context.Response.WriteAsync(new Result()
                            {
                                Messages = new List<string>() { $"Error message - {contextFeature.Error.Message}, inner error message - {(innerException != null ? innerException.Message : "")}" }
                            }.ToJSONString());

                            logger.LogError($"Exception thrown for method {methodPathString}.\r\nStatusCode:\r\n{context.Response.StatusCode}\r\nError message:\r\n{contextFeature.Error.Message}\r\nInner error message:\r\n{(innerException != null ? innerException.Message : "")}");
                        }
                    }
                });
            });

            return app;
        }
    }
}
