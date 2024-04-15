using MudBucket.Commands;
using MudBucket.Interfaces;
using MudBucket.Systems;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;

public class QuitCommand : CommandBase
{
    private readonly Dictionary<TcpClient, PlayerSession> _sessionMap;

    public QuitCommand(Dictionary<TcpClient, PlayerSession> sessionMap)
    {
        _sessionMap = sessionMap;
    }

    protected override async Task<bool> ExecuteCommand(TcpClient client, INetworkService networkService)
    {
        if (_sessionMap.TryGetValue(client, out PlayerSession session))
        {
            await networkService.SendAsync("[bright_green]Goodbye!");
            session.Cleanup();
            return true;
        }
        return false;
    }
}
