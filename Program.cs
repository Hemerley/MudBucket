using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MudBucket.Interfaces;
using MudBucket.Network;
using MudBucket.Services.Commands;
using MudBucket.Services.Logger;
using MudBucket.Services.Ticks;
using MudBucket.Systems;
using System.Net;

namespace MudBucket
{
    class Program
    {
        static void Main(string[] args)
        {
            // Configure services and build the service provider
            var serviceProvider = ConfigureServices();

            // Retrieve required services from the DI container
            var serverManager = serviceProvider.GetRequiredService<ServerManager>();
            var tickScheduler = serviceProvider.GetRequiredService<IScheduler>();

            var combatTick = new CombatTick(serviceProvider.GetRequiredService<ILogger>());
            var repopTick = new RepopTick(serviceProvider.GetRequiredService<ILogger>());
            var worldTick = new WorldTick(serviceProvider.GetRequiredService<ILogger>());

            tickScheduler.ScheduleTickable(combatTick);
            tickScheduler.ScheduleTickable(repopTick);
            tickScheduler.ScheduleTickable(worldTick);

            try
            {
                // Start the scheduler and the server
                tickScheduler.Start();
                serverManager.StartServer();
                Console.WriteLine("Server and scheduler are running. Press any key to stop...");

                Console.ReadKey(); // Wait for user input to stop the server

                // Stop the server and scheduler upon user request
                serverManager.StopServer();
                tickScheduler.Stop();
            }
            catch (Exception ex)
            {
                var logger = serviceProvider.GetRequiredService<ILogger>();
                logger.Error($"An unexpected error occurred: {ex.Message}");
            }
            finally
            {
                // Proper cleanup: Dispose of the service provider and its services
                if (serviceProvider is IDisposable disposable)
                {
                    disposable.Dispose();
                }
                Console.WriteLine("Server and scheduler stopped successfully.");
            }
        }

        private static ServiceProvider ConfigureServices()
        {
            // Create a new instance of service collection
            var services = new ServiceCollection();

            // Add configuration setup
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Initialize logger from configuration
            services.AddSingleton<ILogger>(provider => new SerilogLogger(configuration));

            // Use CommandHandler for handling commands, assuming it implements ICommandParser
            services.AddSingleton<CommandHandler>(); // CommandHandler must implement ICommandParser
            services.AddSingleton<ICommandParser>(provider => provider.GetRequiredService<CommandHandler>());

            // Add scheduler and timer services
            services.AddSingleton<IScheduler, TickScheduler>();
            services.AddSingleton<ITickTimer, GameTickTimer>();

            // Configure and add TCP server
            var ipAddress = IPAddress.Parse(configuration["ApplicationSettings:IPAddress"] ?? "127.0.0.1");
            var port = int.Parse(configuration["ApplicationSettings:Port"] ?? "8888");
            services.AddSingleton(provider =>
                new TcpServer(
                    ipAddress,
                    port,
                    provider.GetRequiredService<ILogger>(),
                    provider.GetRequiredService<CommandHandler>()));

            // Add server manager
            services.AddSingleton<ServerManager>();

            // Build the service provider from the service collection
            return services.BuildServiceProvider();
        }
    }
}
