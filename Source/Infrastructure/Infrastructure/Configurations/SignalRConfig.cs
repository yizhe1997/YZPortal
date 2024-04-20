using Domain.Enums;

namespace Infrastructure.Configurations
{
    public class SignalRConfig
    {
        public SignalRType SignalRType { get; set; }
        public AzureConfig Azure { get; set; } = new();
        public class AzureConfig
        {
            public string? ConnectionString { get; set; }
        }
    }
}
