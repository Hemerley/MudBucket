using MudBucket.Systems;
using System.Net.Sockets;

namespace MudBucket.Interfaces
{
    public interface ICommandParser
    {
        Task<bool> ParseCommand(string command, TcpClient client, PlayerSession session);
    }
}
