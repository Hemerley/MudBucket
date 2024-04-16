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

    public override SessionState[] ValidStates => new[] { SessionState.JustConnected, SessionState.Playing };

    protected override async Task<bool> ExecuteCommand(TcpClient client, INetworkService networkService, PlayerSession session)
    {
        if (_sessionMap.TryGetValue(client, out PlayerSession foundSession))
        {
            // Ensure the session found in the map matches the session passed to the command
            if (session == foundSession)
            {
                await networkService.SendAsync("[white][[server_info]INFO[white]][server]You have been successfully disconnected[white], [server] Goodbye[white]!");
                session.Cleanup();
                return true;
            }
        }
        return false;
    }
}
