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

            // Initialize the tick timer and server
            ITickTimer tickTimer = new GameTickTimer();
            TcpServer server = new TcpServer(ipAddress, appSettings.Port, logger, commandParser);
            ServerManager serverManager = new ServerManager(tickTimer, server);

            // Register tickable entities
            WorldTick worldTick = new WorldTick();
            CombatTick combatTick = new CombatTick(); // Combat tick initialization
            serverManager.RegisterTickable(worldTick);
            serverManager.RegisterTickable(combatTick); // Register the combat tick action

            // Start the server asynchronously
            await StartServer(serverManager);

            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();

            // Stop the server asynchronously
            await StopServer(serverManager);
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

    // Asynchronously start the server to allow the main thread to remain responsive
    private static async Task StartServer(ServerManager serverManager)
    {
        await Task.Run(() => serverManager.StartServer());
    }

    // Asynchronously stop the server
    private static async Task StopServer(ServerManager serverManager)
    {
        await Task.Run(() => serverManager.StopServer());
    }
}
