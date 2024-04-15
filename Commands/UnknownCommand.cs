using MudBucket.Interfaces;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace MudBucket.Commands
{
    public class UnknownCommand : CommandBase
    {
        protected override async Task<bool> ExecuteCommand(TcpClient client, INetworkService networkService)
        {
            await networkService.SendAsync("[white][[server_warning]Warning[white]][server]Arf[white],[server] Arf[white]! [server]Bucket doesn[white]'[server]t understand[white]!");
            return true;
        }
    }
}
