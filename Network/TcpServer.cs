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
        private readonly IStateManager _stateManager;
        private readonly Dictionary<int, PlayerSession> _sessions;
        private int _sessionCounter = 0;

        public TcpServer(IPAddress ipAddress, int port, ILogger logger, ICommandParser commandParser, IMessageFormatter messageFormatter, IStateManager stateManager)
        {
            _ipAddress = ipAddress;
            _port = port;
            _listener = new TcpListener(_ipAddress, _port);
            _commandParser = commandParser;
            _logger = logger;
            _messageFormatter = messageFormatter;
            _stateManager = stateManager;
            _sessions = new Dictionary<int, PlayerSession>();
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
                    var networkService = new NetworkService(client);
                    var session = new PlayerSession(client, networkService, _commandParser, _messageFormatter, _stateManager);
                    _sessions.Add(_sessionCounter++, session);
                    _ = Task.Run(() => HandleClient(session)).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Server encountered an error: {ex.Message}");
                Stop();
            }
        }

        private async Task HandleClient(PlayerSession session)
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
                CloseSession(session);
            }
        }

        private void CloseSession(PlayerSession session)
        {
            int sessionId = _sessions.FirstOrDefault(x => x.Value == session).Key;
            session.Disconnect();
            _sessions.Remove(sessionId);
            _logger.Information("Client disconnected");
        }

        public void Stop()
        {
            _isRunning = false;
            _listener.Stop();
            _logger.Information("Server stopped");
        }
    }
}
