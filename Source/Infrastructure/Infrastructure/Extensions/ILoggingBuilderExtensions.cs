using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;

namespace Infrastructure.Extensions
{
    public static class ILoggingBuilderExtensions
    {
        public static void ConfigureLogging(this ILoggingBuilder builder)
        {
            builder.ClearProviders();
            builder.AddConsole();
            builder.AddDebug();
            builder.AddAzureWebAppDiagnostics();
            builder.ConfigureOpenTelemetry();

            // Ref: https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/wiki/PII
            // https://stackoverflow.com/questions/54435551/invalidoperationexception-idx20803-unable-to-obtain-configuration-from-pii
            IdentityModelEventSource.ShowPII = true;
        }

        public static void ConfigureOpenTelemetry(this ILoggingBuilder builder)
        {
            builder.AddOpenTelemetry(x =>
            {
                x.IncludeFormattedMessage = true;
                x.IncludeScopes = true;
            });
        }
    }
}
