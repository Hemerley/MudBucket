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
            await networkService.SendAsync($"[white][[server_info]INFO[white]][server]Attempting to move you[white]:[server_info] {_direction}");
            return true;
        }
    }
}
