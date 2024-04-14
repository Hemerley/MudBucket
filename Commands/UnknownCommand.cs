using MudBucket.Interfaces;
using System.Net.Sockets;

namespace MudBucket.Commands
{
    public class UnknownCommand : CommandBase
    {
        public override async Task<bool> Execute(TcpClient client, INetworkService networkService)
        {
            await networkService.SendAsync("Arf, Arf! Bucket doesn't understand!");
            return true;
        }
    }
}