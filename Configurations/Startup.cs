﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MudBucket;
using MudBucket.Commands;
using MudBucket.Configurations;
using MudBucket.Interfaces;
using MudBucket.Network;
using MudBucket.Structures;
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
        services.AddSingleton<GameDataRepository>(provider => GameDataRepository.Instance);
        services.AddSingleton<IMessageFormatter>(provider => new MessageFormatter(true));
        var ipAddress = IPAddress.Parse(configuration["ApplicationSettings:IPAddress"] ?? "127.0.0.1");
        var port = int.Parse(configuration["ApplicationSettings:Port"] ?? "8888");
        services.AddSingleton<TcpServer>(provider =>
            new TcpServer(
                ipAddress,
                port,
                provider.GetRequiredService<ILogger>(),
                provider.GetRequiredService<CommandHandler>(),
                provider.GetRequiredService<IMessageFormatter>()));
        services.AddSingleton<ServerManager>();
    }
}