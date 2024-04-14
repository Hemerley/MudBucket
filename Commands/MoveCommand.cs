using MudBucket.Interfaces;
using System.Net.Sockets;

namespace MudBucket.Commands
{
    public class MoveCommand : CommandBase
    {
        private readonly string _direction;

        public MoveCommand(string direction)
        {
            _direction = direction;
        }

        public override async Task<bool> Execute(TcpClient client, INetworkService networkService)
        {
            await networkService.SendAsync($"[yellow]Attempting to move you: {_direction}");
            return true;
        }
    }
}
