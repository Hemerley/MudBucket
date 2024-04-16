using MudBucket.Interfaces;
using MudBucket.Services.General;
using MudBucket.Systems;
using System.Net.Sockets;

namespace MudBucket.Commands
{
    public class NewCommand : CommandBase
    {
        public override SessionState[] ValidStates => new[] { SessionState.JustConnected };

        protected override async Task<bool> ExecuteCommand(TcpClient client, INetworkService networkService, PlayerSession session)
        {
            // Placeholder for new player logic
            await networkService.SendAsync("[white][[server_info]INFO[white]][server]Creating a new character...");
            return true;
        }
    }
}