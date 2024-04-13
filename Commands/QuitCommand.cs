using System.Net.Sockets;

namespace MudBucket.Commands
{
    public class QuitCommand : CommandBase
    {
        public override bool Execute(TcpClient client)
        {
            SendToClient("Goodbye!", client);
            return true;
        }
    }
}
