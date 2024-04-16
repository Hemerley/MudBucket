using MudBucket.Systems;
using System.Net.Sockets;

namespace MudBucket.Interfaces
{
    public interface ICommand
    {
        SessionState[] ValidStates { get; }
        Task<bool> Execute(TcpClient client, INetworkService networkService, PlayerSession session);
    }
}
