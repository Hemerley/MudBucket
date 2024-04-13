using MudBucket.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MudBucket.Commands
{
    public class MoveCommand : ICommand
    {
        private readonly string _direction;

        public MoveCommand(string direction)
        {
            _direction = direction;
        }

        public bool Execute(TcpClient client)
        {
            SendToClient($"Moving {_direction}", client);
            return true; // Keep the connection open
        }

        private void SendToClient(string message, TcpClient client)
        {
            var stream = client.GetStream();
            var buffer = System.Text.Encoding.ASCII.GetBytes(message + "\r\n");
            stream.Write(buffer, 0, buffer.Length);
        }
    }
}
