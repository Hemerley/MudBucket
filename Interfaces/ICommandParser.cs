using System.Net.Sockets;

namespace MudBucket.Interfaces
{
    public interface ICommandParser
    {
        bool ParseCommand(string command, TcpClient client);
    }
}
