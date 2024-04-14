using MudBucket.Interfaces;
using MudBucket.Services.Commands;
using MudBucket.Services.Server;
using MudBucket.Systems;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

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
        private readonly IStateManager _stateManager;
        private readonly Dictionary<TcpClient, PlayerSession> _sessions;

        public TcpServer(IPAddress ipAddress, int port, ILogger logger, ICommandParser commandParser, IMessageFormatter messageFormatter, IStateManager stateManager)
        {
            _ipAddress = ipAddress;
            _port = port;
            _listener = new TcpListener(_ipAddress, _port);
            _commandParser = commandParser;
            _logger = logger;
            _messageFormatter = messageFormatter;
            _stateManager = stateManager;
            _sessions = new Dictionary<TcpClient, PlayerSession>();
            CommandFactory.Initialize(_sessions); // Initialize CommandFactory with the session map
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
                    // Ensuring NetworkService is properly initialized with a message formatter
                    var networkService = new NetworkService(client, _messageFormatter);
                    var session = new PlayerSession(client, networkService, _commandParser, _messageFormatter, _stateManager);
                    _sessions.Add(client, session);
                    _ = Task.Run(() => HandleClient(client, session)).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Server encountered an error: {ex.Message}");
                Stop();
            }
        }

        private async Task HandleClient(TcpClient client, PlayerSession session)
        {
            try
            {
                await session.HandleSession();
            }
            catch (Exception ex)
            {
                _logger.Error($"Error handling client session: {ex.Message}");
            }
            finally
            {
                CloseSession(client);
            }
        }

        private void CloseSession(TcpClient client)
        {
            if (_sessions.TryGetValue(client, out PlayerSession session))
            {
                session.Cleanup();
                _sessions.Remove(client);
                _logger.Information("Client disconnected");
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
    }
}
