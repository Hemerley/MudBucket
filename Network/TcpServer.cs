using MudBucket.Interfaces;
using MudBucket.Services.Server;
using MudBucket.Systems;
using System.Net;
using System.Net.Sockets;

namespace MudBucket.Network
{
    public class TcpServer
    {
        private readonly IPAddress _ipAddress;
        private readonly int _port;
        private TcpListener _listener;
        private bool _isRunning;
        private readonly ICommandParser _commandParser;
        private readonly ILogger _logger;
        private readonly IMessageFormatter _messageFormatter;
        private readonly Dictionary<TcpClient, PlayerSession> _sessions;
        public TcpServer(IPAddress ipAddress, int port, ILogger logger, ICommandParser commandParser, IMessageFormatter messageFormatter)
        {
            _ipAddress = ipAddress;
            _port = port;
            _listener = new TcpListener(_ipAddress, _port);
            _commandParser = commandParser;
            _logger = logger;
            _messageFormatter = messageFormatter;
            _sessions = new Dictionary<TcpClient, PlayerSession>();
        }
        public bool IsRunning => _isRunning;
        public async Task Start()
        {
            _listener.Start();
            _isRunning = true;
            _logger.Information($"Server started on port {_port}");

            try
            {
                while (_isRunning)
                {
                    TcpClient client = await _listener.AcceptTcpClientAsync();
                    _logger.Information("Client connected");
                    var networkService = new NetworkService(client, _messageFormatter);
                    var session = new PlayerSession(client, networkService, _commandParser, _messageFormatter);
                    _sessions.Add(client, session);
                    _ = Task.Run(() => session.HandleSession());
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Server encountered an error: {ex.Message}");
                Stop();
            }
        }
        public void Stop()
        {
            _isRunning = false;
            _listener.Stop();
            foreach (var client in new List<TcpClient>(_sessions.Keys))
            {
                CloseSession(client);
            }
            _logger.Information("Server stopped");
        }
        public void BroadcastMessage(string message)
        {
            foreach (var session in _sessions.Values)
            {
                _ = session.SendMessageAsync(message);
            }
        }
        private void CloseSession(TcpClient client)
        {
            if (_sessions.TryGetValue(client, out var session))
            {
                session.Cleanup();
                _sessions.Remove(client);
                _logger.Information("Client disconnected");
            }
        }
        public Dictionary<TcpClient, PlayerSession> Sessions => _sessions;
    }
}