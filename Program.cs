using Microsoft.Extensions.DependencyInjection;
using MudBucket.Commands;
using MudBucket.Interfaces;
using MudBucket.Network;
using MudBucket.Ticks;

namespace MudBucket
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serviceProvider = ConfigureServices();
            var serverManager = serviceProvider.GetRequiredService<ServerManager>();
            var tickScheduler = serviceProvider.GetRequiredService<IScheduler>();
            var server = serviceProvider.GetRequiredService<TcpServer>();
            var logger = serviceProvider.GetRequiredService<ILogger>();
            tickScheduler.ScheduleTickable(new CombatTick(serviceProvider.GetRequiredService<ILogger>()));
            tickScheduler.ScheduleTickable(new RepopTick(serviceProvider.GetRequiredService<ILogger>()));
            tickScheduler.ScheduleTickable(new WorldTick(serviceProvider.GetRequiredService<ILogger>()));
            CommandFactory.Initialize(server.Sessions);
            try
            {
                tickScheduler.Start();
                serverManager.StartServer();
                logger.Information("Server and scheduler are running. Type 'shutdown' to shut down gracefully, 'abort' to abort the shutdown, or 'quit' to exit immediately.");
                await Task.Run(() => ListenForConsoleCommands(serverManager, server));
            }
            catch (Exception ex)
            {
                logger.Error($"An unexpected error occurred: {ex.Message}");
            }
            finally
            {
                if (serviceProvider is IDisposable disposable)
                {
                    disposable.Dispose();
                }
                logger.Information("Server and scheduler are stopping...");
                logger.Information("Server and scheduler stopped successfully.");
            }
        }
        private static void ListenForConsoleCommands(ServerManager serverManager, TcpServer server)
        {
            var serviceProvider = ConfigureServices();
            var logger = serviceProvider.GetRequiredService<ILogger>();
            while (true)
            {
                var command = Console.ReadLine()?.Trim().ToLower();
                switch (command)
                {
                    case "shutdown":
                        serverManager.InitiateShutdown();
                        break;
                    case "abort":
                        serverManager.AbortShutdown();
                        break;
                    case "quit":
                        server.BroadcastMessage("[white][[server_info]INFO[white]][server]Server is shutting down immediately[white]. [server]Thank you for playing[white]!");
                        logger.Information("Server is shutting down immediately.");
                        Environment.Exit(0);
                        break;
                    default:
                        logger.Information("Unknown command: " + command + ". Available commands: shutdown, abort, quit");
                        break;
                }
            }
        }
        private static ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            Startup.ConfigureServices(services);
            return services.BuildServiceProvider();
        }
    }
}