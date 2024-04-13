using MudBucket.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MudBucket.Commands
{
    public class LookCommand : ICommand
    {
        public bool Execute(TcpClient client)
        {
            SendToClient("You look around and see...", client);
            return true;
        }

        private void SendToClient(string message, TcpClient client)
        {
            var stream = client.GetStream();
            var buffer = System.Text.Encoding.ASCII.GetBytes(message + "\r\n");
            stream.Write(buffer, 0, buffer.Length);
        }
    }
}
