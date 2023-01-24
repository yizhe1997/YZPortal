using Azure.Storage.Blobs;
using System.Net;
using YZPortal.Core.Error;

namespace YZPortal.API.Infrastructure.AzureStorage
{
    public static class StartupExtensions
    {
        public static void AddAzureStorage(this IServiceCollection services, IConfiguration configuration)
        {
            // Inject logging service
            var serviceProvider = new ServiceCollection()
                                  .AddLogging(cfg => cfg.AddConsole())
                                  .Configure<LoggerFilterOptions>(cfg => cfg.MinLevel = LogLevel.Information)
                                  .BuildServiceProvider();
            var logger = serviceProvider.GetService<ILogger<AzureStorageOptions>>() ?? throw new RestException(HttpStatusCode.InternalServerError, $"Failed to get service of type Ilogger<{nameof(AzureStorageOptions)}>");

            services.Configure<AzureStorageOptions>(configuration.GetSection("AzureStorage"));

            services.AddScoped(x =>
            {
                try
                {
                    string connectionString = configuration.GetConnectionString("AzureStorage") ?? string.Empty;

                    // Create a BlobServiceClient object which will be used to create a container client
                    BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

                    return blobServiceClient;
                }
                catch (Exception ex)
                {
                    var errorMsg = $"An error occured when creating Microsoft.Azure.Storage.CloudStorageAccount using {nameof(AzureStorageOptions)} configuration - {ex.Message}";
                    logger.LogError(errorMsg);
                    throw new RestException(HttpStatusCode.InternalServerError, errorMsg);
                }
            });
        }
    }
}
