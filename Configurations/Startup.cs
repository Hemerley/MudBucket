using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MudBucket;
using MudBucket.Configurations;
using MudBucket.Interfaces;
using MudBucket.Interfaces.MudBucket.Interfaces;
using MudBucket.Network;
using MudBucket.Services.Commands;
using MudBucket.Services.General;
using MudBucket.Systems;
using System.Net;

public class Startup
{
    public static void ConfigureServices(IServiceCollection services)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        services.AddSingleton<ILogger>(provider => LoggerConfig.InitializeLogger(configuration));

        services.AddSingleton<CommandHandler>();
        services.AddSingleton<ICommandParser>(provider => provider.GetRequiredService<CommandHandler>());

        services.AddSingleton<IScheduler, TickScheduler>();
        services.AddSingleton<ITickTimer, GameTickTimer>();

        services.AddSingleton<IMessageFormatter>(provider => new MessageFormatter(true));
        // Removed IStateManager, StateManager

        var ipAddress = IPAddress.Parse(configuration["ApplicationSettings:IPAddress"] ?? "127.0.0.1");
        var port = int.Parse(configuration["ApplicationSettings:Port"] ?? "8888");
        services.AddSingleton<TcpServer>(provider =>
            new TcpServer(
                ipAddress,
                port,
                provider.GetRequiredService<ILogger>(),
                provider.GetRequiredService<ICommandParser>(),
                provider.GetRequiredService<IMessageFormatter>()));

        services.AddSingleton<IDataPersistenceService, DataPersistenceService>();

        services.AddSingleton<ServerManager>();
    }
}
