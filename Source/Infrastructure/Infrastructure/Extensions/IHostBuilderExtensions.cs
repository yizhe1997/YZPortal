using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Infrastructure.Extensions
{
    public static class IHostBuilderExtensions
    {
        public static IHostBuilder UseSerilog(this IHostBuilder builder, IConfiguration configuration)
        {
            // Ref: https://www.codeproject.com/Articles/5344667/Logging-with-Serilog-in-ASP-NET-Core-Web-API#:~:text=Create%20ASP.NET%20Core%20Web%20API%20Project&text=Choose%20.,then%20choose%20to%20install%20Serilog.
            var logger = new LoggerConfiguration()
                                   .ReadFrom.Configuration(configuration)
                                   .Enrich.FromLogContext()
                                   .WriteTo.Console()
                                   .CreateLogger();

            builder.UseSerilog(logger);

            return builder;
        }
    }
}
