using System.Net.Sockets;

namespace MudBucket.Commands
{
    public class UnknownCommand : CommandBase
    {
        public override bool Execute(TcpClient client)
        {
            SendToClient("Arf, Arf! Bucket doesn't understand!", client);
            return true;
        }
    }
}