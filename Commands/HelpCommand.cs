using MudBucket.Interfaces;
using MudBucket.Services.General;
using MudBucket.Systems;
using System.Net.Sockets;

namespace MudBucket.Commands
{
    public class HelpCommand : CommandBase
    {
        public override SessionState[] ValidStates => new[] { SessionState.Playing };

        protected override async Task<bool> ExecuteCommand(TcpClient client, INetworkService networkService, PlayerSession session)
        {
            // Placeholder for help command logic
            await networkService.SendAsync("[white][[server_info]INFO[white]][server]Displaying help files...");
            return true;
        }
    }
}