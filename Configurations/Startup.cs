using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MudBucket.Configurations;
using MudBucket;
using MudBucket.Interfaces;
using MudBucket.Network;
using MudBucket.Services.Commands;
using MudBucket.Services.General;
using MudBucket.Services.Logger;
using MudBucket.Services.Server;
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

        // Command handling and session management
        services.AddSingleton<CommandHandler>();
        services.AddSingleton<ICommandParser>(provider => provider.GetRequiredService<CommandHandler>());

        // Scheduling for tick-based operations
        services.AddSingleton<IScheduler, TickScheduler>();
        services.AddSingleton<ITickTimer, GameTickTimer>();

        // Formatters and state management
        services.AddSingleton<IMessageFormatter>(provider => new MessageFormatter(true));
        services.AddSingleton<IStateManager, StateManager>();

        // Network setup
        var ipAddress = IPAddress.Parse(configuration["ApplicationSettings:IPAddress"] ?? "127.0.0.1");
        var port = int.Parse(configuration["ApplicationSettings:Port"] ?? "8888");
        services.AddSingleton<TcpServer>(provider =>
            new TcpServer(
                ipAddress,
                port,
                provider.GetRequiredService<ILogger>(),
                provider.GetRequiredService<ICommandParser>(),
                provider.GetRequiredService<IMessageFormatter>(),
                provider.GetRequiredService<IStateManager>()));

        // Server management
        services.AddSingleton<ServerManager>();
    }
}
