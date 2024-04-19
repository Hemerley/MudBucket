using MudBucket.Structures;

namespace MudBucket.Interfaces
{
    public interface INetworkService
    {
        Task SendAsync(string message, Player player);
        Task<string> ReceiveAsync();
        void Close();
    }
}
