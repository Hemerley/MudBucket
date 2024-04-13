using Serilog;
using System.Net;
using Microsoft.Extensions.Configuration;
using MudBucket;
using MudBucket.Interfaces;
using MudBucket.Network;
using MudBucket.Systems;

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

        // Ensure that logger starts correctly and close if it fails to initialize
        try
        {
            Log.Information("Starting server...");

            // Deserialize settings
            var appSettings = configuration.GetSection("ApplicationSettings").Get<ApplicationSettings>();

            // Convert string IP address to enum or IP object
            IPAddress ipAddress = appSettings.IPAddress.Equals("Any", StringComparison.OrdinalIgnoreCase) ?
                                  IPAddress.Any : IPAddress.Parse(appSettings.IPAddress);

            // Initialize logger and server with configuration settings
            MudBucket.Interfaces.ILogger logger = new SerilogLogger(); // Custom logger that wraps Serilog
            ICommandParser commandParser = new CommandParser(); // Assuming you have a command parser implementation

            var server = new TcpServer(ipAddress, appSettings.Port, appSettings.BufferSize, logger, commandParser);

            // Start server asynchronously
            await server.StartAsync();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Failed to start server");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}

