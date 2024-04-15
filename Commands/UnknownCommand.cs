using MudBucket.Interfaces;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace MudBucket.Commands
{
    public class UnknownCommand : CommandBase
    {
        protected override async Task<bool> ExecuteCommand(TcpClient client, INetworkService networkService)
        {
            await networkService.SendAsync("[bright_red]Arf, Arf! Bucket doesn't understand!");
            return true;
        }
    }
}
