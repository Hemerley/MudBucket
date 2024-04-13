using Microsoft.Extensions.Configuration;
using MudBucket;
using MudBucket.Interfaces;
using MudBucket.Network;
using MudBucket.Services.Commands;
using MudBucket.Services.Logger;
using MudBucket.Services.Ticks;
using MudBucket.Systems;
using Serilog;
using System.Net;

class Program
{
    static async Task Main(string[] args)
    {
        // Load configuration from appsettings.json
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        // Configure and initialize Serilog for logging
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        try
        {
            Log.Information("Starting server...");
            var appSettings = configuration.GetSection("ApplicationSettings").Get<ApplicationSettings>();

            // Determine the IP address to bind the server to
            IPAddress ipAddress = IPAddress.Parse(appSettings.IPAddress.Equals("Any", StringComparison.OrdinalIgnoreCase) ? "0.0.0.0" : appSettings.IPAddress);

            // Setup logger and command parser
            MudBucket.Interfaces.ILogger logger = new SerilogLogger();
            ICommandParser commandParser = new CommandParser();

            // Initialize the tick scheduler, timer, and server
            var tickScheduler = new TickScheduler(); // Create an instance of TickScheduler
            ITickTimer tickTimer = new GameTickTimer(tickScheduler); // Pass the scheduler to GameTickTimer
            TcpServer server = new TcpServer(ipAddress, appSettings.Port, logger, commandParser);
            ServerManager serverManager = new ServerManager(tickScheduler, server);

            // Register tickable entities
            WorldTick worldTick = new WorldTick(logger);
            CombatTick combatTick = new CombatTick(logger);
            RepopTick repopTick = new RepopTick(logger);

            serverManager.RegisterTickable(worldTick);
            serverManager.RegisterTickable(combatTick);
            serverManager.RegisterTickable(repopTick);

            // Start the server
            serverManager.StartServer();

            // Start the tick scheduler
            tickScheduler.Start();

            Log.Information("Press any key to stop the server...");
            Console.ReadKey();

            // Stop the server
            serverManager.StopServer();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Failed to start server");
        }
        finally
        {
            // Ensure the log buffer is flushed before the application exits
            Log.CloseAndFlush();
        }
    }
}
