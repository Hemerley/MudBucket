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

        public override bool Execute(TcpClient client)
        {
            SendToClient($"Moving {_direction}", client);
            return true;
        }
    }
}
