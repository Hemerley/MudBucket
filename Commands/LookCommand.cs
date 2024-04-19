using MudBucket.Interfaces;
using MudBucket.Systems;
using System.Net.Sockets;

namespace MudBucket.Commands
{
    public class LookCommand : CommandBase
    {
        public override SessionState[] ValidStates => new[] { SessionState.JustConnected, SessionState.Playing };
        protected override async Task<bool> ExecuteCommand(TcpClient client, INetworkService networkService, PlayerSession session)
        {
            await networkService.SendAsync("[white][[server_info]INFO[white]][server]You look around and see[white]...", session.player);
            session.player.CurrentRoom.DescribeRoom(session.player);
            return true;
        }
    }
}