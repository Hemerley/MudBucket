using Microsoft.Extensions.Configuration;
using MudBucket.Systems;
using Serilog;

namespace MudBucket.Configurations
{
    public static class LoggerConfig
    {
        public static Interfaces.ILogger InitializeLogger(IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
            return new SerilogLogger(configuration);
        }
    }
}