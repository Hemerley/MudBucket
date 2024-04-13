using MudBucket.Interfaces;
using System.Net.Sockets;
using System.Text;

namespace MudBucket.Commands
{
    public abstract class CommandBase : ICommand
    {
        protected void SendToClient(string message, TcpClient client)
        {
            var stream = client.GetStream();
            var buffer = Encoding.ASCII.GetBytes(message + "\r\n");
            stream.Write(buffer, 0, buffer.Length);
        }

        public abstract bool Execute(TcpClient client);
    }
}
