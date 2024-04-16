using Microsoft.Extensions.DependencyInjection;
using MudBucket.Interfaces;
using MudBucket.Network;
using MudBucket.Services.Commands;
using MudBucket.Services.Ticks;

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
            tickScheduler.ScheduleTickable(new CombatTick(serviceProvider.GetRequiredService<ILogger>()));
            tickScheduler.ScheduleTickable(new RepopTick(serviceProvider.GetRequiredService<ILogger>()));
            tickScheduler.ScheduleTickable(new WorldTick(serviceProvider.GetRequiredService<ILogger>()));
            CommandFactory.Initialize(server.Sessions);
            try
            {
                tickScheduler.Start();
                serverManager.StartServer();
                Console.WriteLine("Server and scheduler are running. Type 'shutdown' to shut down gracefully, 'abort' to abort the shutdown, or 'quit' to exit immediately.");
                await Task.Run(() => ListenForConsoleCommands(serverManager, server));
            }
            catch (Exception ex)
            {
                var logger = serviceProvider.GetRequiredService<ILogger>();
                logger.Error($"An unexpected error occurred: {ex.Message}");
            }
            finally
            {
                if (serviceProvider is IDisposable disposable)
                {
                    disposable.Dispose();
                }
                Console.WriteLine("Server and scheduler stopped successfully.");
            }
        }
        private static void ListenForConsoleCommands(ServerManager serverManager, TcpServer server)
        {
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
                        Console.WriteLine("Exiting immediately...");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Unknown command. Available commands: shutdown, abort, quit");
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