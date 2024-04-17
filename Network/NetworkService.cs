using MudBucket.Interfaces;
using System.Net.Sockets;
using System.Text;

namespace MudBucket.Network
{
    public class NetworkService : INetworkService
    {
        private readonly TcpClient _client;
        private readonly IMessageFormatter _messageFormatter;
        public NetworkService(TcpClient client, IMessageFormatter messageFormatter)
        {
            _client = client;
            _messageFormatter = messageFormatter;
        }
        public async Task SendAsync(string message)
        {
            message = _messageFormatter.FormatMessage(message);
            var buffer = Encoding.ASCII.GetBytes(message);
            await _client.GetStream().WriteAsync(buffer, 0, buffer.Length);
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