namespace MudBucket.Interfaces
{
    public interface INetworkService
    {
        Task SendAsync(string message);
        Task<string> ReceiveAsync();
        void Close();
    }
}
