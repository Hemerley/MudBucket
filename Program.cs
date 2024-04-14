using Microsoft.Extensions.DependencyInjection;
using MudBucket.Interfaces;
using MudBucket.Systems;
using MudBucket.Network;
using Microsoft.Extensions.Configuration;
using MudBucket.Services.Commands;
using MudBucket.Configurations;
using MudBucket.Services.Ticks;

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

            var combatTick = new CombatTick(serviceProvider.GetRequiredService<Interfaces.ILogger>());
            var repopTick = new RepopTick(serviceProvider.GetRequiredService<Interfaces.ILogger>());
            var worldTick = new WorldTick(serviceProvider.GetRequiredService<Interfaces.ILogger>());

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
                var logger = serviceProvider.GetRequiredService<Interfaces.ILogger>();
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

            // Add logging service
            services.AddSingleton<Interfaces.ILogger>(LoggerConfig.InitializeLogger(configuration));

            // Add other services required by the application
            services.AddSingleton<ICommandParser, CommandParser>();
            services.AddSingleton<IScheduler, TickScheduler>();
            services.AddSingleton<ITickTimer, GameTickTimer>();
            services.AddSingleton<TcpServer>(provider =>
            {
                var ipAddress = System.Net.IPAddress.Parse(configuration["ApplicationSettings:IPAddress"] ?? "127.0.0.1");
                var port = int.Parse(configuration["ApplicationSettings:Port"] ?? "8888");
                return new TcpServer(
                    ipAddress,
                    port,
                    provider.GetRequiredService<Interfaces.ILogger>(),
                    provider.GetRequiredService<ICommandParser>());
            });

            services.AddSingleton<ServerManager>();

            // Build the service provider from the service collection
            return services.BuildServiceProvider();
        }
    }
}
