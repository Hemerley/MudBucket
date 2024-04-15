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

    protected override async Task<bool> ExecuteCommand(TcpClient client, INetworkService networkService)
    {
        if (_sessionMap.TryGetValue(client, out PlayerSession session))
        {
            await networkService.SendAsync("[white][[server_info]INFO[white]][server]You have been successfully disconnected[white],[server] Goodbye[white]!");
            session.Cleanup();
            return true;
        }
        return false;
    }
}
