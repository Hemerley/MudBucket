﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MudBucket;
using MudBucket.Interfaces;
using MudBucket.Network;
using MudBucket.Services.Commands;
using MudBucket.Services.Logger;
using MudBucket.Systems;
using System.Net;

public class Startup
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ILogger>(provider =>
        {
            var loggerConfiguration = new ConfigurationBuilder()
                                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                        .Build();
            return new SerilogLogger(loggerConfiguration);
        });
        services.AddSingleton<CommandHandler>();
        services.AddSingleton<IScheduler, TickScheduler>();
        services.AddSingleton<ITickTimer, GameTickTimer>();

        var ipAddress = IPAddress.Parse(configuration["ApplicationSettings:IPAddress"] ?? "127.0.0.1");
        var port = int.Parse(configuration["ApplicationSettings:Port"] ?? "8888");

        services.AddSingleton<TcpServer>(provider =>
            new TcpServer(
                ipAddress,
                port,
                provider.GetRequiredService<ILogger>(),
                provider.GetRequiredService<CommandHandler>()
            ));

        services.AddSingleton<ServerManager>();
    }
}
