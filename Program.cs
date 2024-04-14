using Microsoft.Extensions.DependencyInjection;
using MudBucket.Interfaces;
using MudBucket.Services.Ticks;

namespace MudBucket
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = ConfigureServices();
            var serverManager = serviceProvider.GetRequiredService<ServerManager>();
            var tickScheduler = serviceProvider.GetRequiredService<IScheduler>();

            tickScheduler.ScheduleTickable(new CombatTick(serviceProvider.GetRequiredService<ILogger>()));
            tickScheduler.ScheduleTickable(new RepopTick(serviceProvider.GetRequiredService<ILogger>()));
            tickScheduler.ScheduleTickable(new WorldTick(serviceProvider.GetRequiredService<ILogger>()));

            try
            {
                tickScheduler.Start();
                serverManager.StartServer();
                Console.WriteLine("Server and scheduler are running. Press any key to stop...");

                Console.ReadKey();
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
                if (serviceProvider is IDisposable disposable)
                {
                    disposable.Dispose();
                }
                Console.WriteLine("Server and scheduler stopped successfully.");
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
