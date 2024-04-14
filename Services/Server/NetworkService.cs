using MudBucket.Interfaces;
using System.Net.Sockets;
using System.Text;

namespace MudBucket.Services.Server
{
    public class NetworkService : INetworkService
    {
        private readonly TcpClient _client;

        public NetworkService(TcpClient client)
        {
            _client = client;
        }

        public async Task SendAsync(string message)
        {
            var stream = _client.GetStream();
            var buffer = Encoding.ASCII.GetBytes(message);
            await stream.WriteAsync(buffer, 0, buffer.Length);
            await stream.FlushAsync();
        }

        public async Task<string> ReceiveAsync()
        {
            var stream = _client.GetStream();
            var buffer = new byte[1024];
            var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            return Encoding.ASCII.GetString(buffer, 0, bytesRead);
        }

        public void Close()
        {
            _client.Close();
        }
    }
}
