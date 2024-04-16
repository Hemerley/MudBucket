using MudBucket.Systems;
using System.Net.Sockets;

namespace MudBucket.Interfaces
{
    public interface ICommandParser
    {
        // Updated to include necessary parameters
        Task<bool> ParseCommand(string command, TcpClient client, PlayerSession session);
    }
}
