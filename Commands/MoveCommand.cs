using MudBucket.Interfaces;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace MudBucket.Commands
{
    public class MoveCommand : CommandBase
    {
        private readonly string _direction;

        public MoveCommand(string direction)
        {
            _direction = direction;
        }

        protected override async Task<bool> ExecuteCommand(TcpClient client, INetworkService networkService)
        {
            await networkService.SendAsync($"[bright_yellow]Attempting to move you: {_direction}");
            return true;
        }
    }
}
