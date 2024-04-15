using MudBucket.Interfaces;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace MudBucket.Commands
{
    public class LookCommand : CommandBase
    {
        protected override async Task<bool> ExecuteCommand(TcpClient client, INetworkService networkService)
        {
            await networkService.SendAsync("[white][[server_info]INFO[white]][server]You look around and see[white]...");
            return true;
        }
    }
}
