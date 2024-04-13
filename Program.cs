using Serilog;
using System.Net;
using Microsoft.Extensions.Configuration;
using MudBucket;
using MudBucket.Interfaces;

class Program
{
    static async Task Main(string[] args)
    {
        // Build configuration
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        // Configure Serilog
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        // Deserialize settings
        var appSettings = configuration.GetSection("ApplicationSettings").Get<ApplicationSettings>();

        // Convert string IP address to enum or IP object
        IPAddress ipAddress = appSettings.IPAddress.Equals("Any", StringComparison.OrdinalIgnoreCase) ?
                              IPAddress.Any : IPAddress.Parse(appSettings.IPAddress);

        // Initialize logger and server with configuration settings
        MudBucket.Interfaces.ILogger logger = new SerilogLogger();
        var server = new TcpServer(ipAddress, appSettings.Port, appSettings.BufferSize, appSettings.ConnectionTimeout, logger);

        await server.StartAsync();
    }
}
