using MudBucket.Interfaces;
using System.Net.Sockets;
using System.Text;

namespace MudBucket.Commands
{
    public abstract class CommandBase : ICommand
    {
        public abstract Task<bool> Execute(TcpClient client, INetworkService networkService);
    }
}
