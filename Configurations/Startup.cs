using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MudBucket.Interfaces;
using MudBucket.Network;
using MudBucket.Services.Commands;
using MudBucket.Systems;
using MudBucket;
using MudBucket.Configurations;
using Serilog;
using System.Net;

public class Startup
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<MudBucket.Interfaces.ILogger>((MudBucket.Interfaces.ILogger)LoggerConfig.InitializeLogger(configuration));
        services.AddSingleton<ICommandParser, CommandParser>();
        services.AddSingleton<IScheduler, TickScheduler>();
        services.AddSingleton<ITickTimer, GameTickTimer>();

        var ipAddress = IPAddress.Parse(configuration["ApplicationSettings:IPAddress"] ?? "127.0.0.1");
        var port = int.Parse(configuration["ApplicationSettings:Port"] ?? "8888");
        services.AddSingleton<TcpServer>(provider =>
            new TcpServer(
                ipAddress,
                port,
                provider.GetRequiredService<MudBucket.Interfaces.ILogger>(),
                provider.GetRequiredService<ICommandParser>()
            ));

        services.AddSingleton<ServerManager>();
    }
}
