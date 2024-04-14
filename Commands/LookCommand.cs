using MudBucket.Interfaces;
using System.Net.Sockets;

namespace MudBucket.Commands
{
    public class LookCommand : CommandBase
    {
        public override async Task<bool> Execute(TcpClient client, INetworkService networkService)
        {
            await networkService.SendAsync("[yellow]You look around and see...");
            return true;
        }
    }
}
