using MudBucket.Commands;
using MudBucket.Interfaces;
using MudBucket.Systems;
using System.Net.Sockets;

public class QuitCommand : CommandBase
{
    private readonly Dictionary<TcpClient, PlayerSession> _sessionMap;

    public QuitCommand(Dictionary<TcpClient, PlayerSession> sessionMap)
    {
        _sessionMap = sessionMap;
    }

    public override async Task<bool> Execute(TcpClient client, INetworkService networkService)
    {
        if (_sessionMap.TryGetValue(client, out PlayerSession session))
        {
            await networkService.SendAsync("[green]Goodbye!");
            session.Cleanup();
            return true;
        }
        return false;
    }
}