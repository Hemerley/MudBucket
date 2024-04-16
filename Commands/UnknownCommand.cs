using MudBucket.Interfaces;
using MudBucket.Systems;
using System.Net.Sockets;

namespace MudBucket.Commands
{
    public class UnknownCommand : CommandBase
    {
        public override SessionState[] ValidStates => new[] { SessionState.JustConnected, SessionState.Playing };

        protected override async Task<bool> ExecuteCommand(TcpClient client, INetworkService networkService, PlayerSession session)
        {
            await networkService.SendAsync("[white][[server_warning]Warning[white]][server]Arf[white],[server] Arf[white]! [server]Bucket doesn[white]'[server]t understand[white]!");
            return true;
        }
    }
}
