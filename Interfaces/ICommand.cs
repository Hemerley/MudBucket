using System.Net.Sockets;

namespace MudBucket.Interfaces
{
    public interface ICommand
    {
        bool Execute(TcpClient client);
    }
}
