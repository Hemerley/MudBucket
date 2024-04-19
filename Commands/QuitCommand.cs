using MudBucket.Interfaces;
using MudBucket.Systems;
using System.Net.Sockets;

namespace MudBucket.Commands
{
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
                if (session == foundSession)
                {
                    await networkService.SendAsync("[white][[server_info]INFO[white]][server]You have been successfully disconnected[white], [server] Goodbye[white]!", session.player);
                    session.Cleanup();
                    return true;
                }
            }
            return false;
        }
    }
}