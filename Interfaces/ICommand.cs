using MudBucket.Interfaces;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace MudBucket.Interfaces
{
    public interface ICommand
    {
        Task<bool> Execute(TcpClient client, INetworkService networkService);
    }
}
