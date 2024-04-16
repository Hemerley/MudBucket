using MudBucket.Interfaces;
using MudBucket.Systems;
using System.Net.Sockets;

namespace MudBucket.Commands
{
    public class MoveCommand : CommandBase
    {
        private readonly string _direction;
        public override SessionState[] ValidStates => new[] { SessionState.Playing };

        public MoveCommand(string direction)
        {
            _direction = direction;
        }

        protected override async Task<bool> ExecuteCommand(TcpClient client, INetworkService networkService, PlayerSession session)
        {
            await networkService.SendAsync($"[white][[server_info]INFO[white]][server]Attempting to move you[white]:[server_info] {_direction}");
            return true;
        }
    }
}
