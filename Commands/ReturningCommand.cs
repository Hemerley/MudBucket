using MudBucket.Interfaces;
using MudBucket.Systems;
using System.Net.Sockets;

namespace MudBucket.Commands
{
    public class ReturningCommand : CommandBase
    {
        public override SessionState[] ValidStates => new[] { SessionState.JustConnected };
        protected override async Task<bool> ExecuteCommand(TcpClient client, INetworkService networkService, PlayerSession session)
        {
            await networkService.SendAsync("[white][[server_info]INFO[white]][server]Welcome back!", session.player);
            return true;
        }
    }
}