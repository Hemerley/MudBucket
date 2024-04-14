using MudBucket.Interfaces;
using MudBucket.Services.Commands;  // Assuming PlayerSession is located in this namespace
using MudBucket.Systems;  // Assuming you have a logger service
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MudBucket.Network
{
    public class TcpServer
    {
        private readonly IPAddress _ipAddress;
        private readonly int _port;
        private TcpListener _listener;
        private bool _isRunning;
        private readonly CommandHandler _commandHandler;
        private readonly ILogger _logger;
        private readonly Dictionary<int, PlayerSession> _sessions;
        private int _sessionCounter = 0;

        public TcpServer(IPAddress ipAddress, int port, ILogger logger, CommandHandler commandHandler)
        {
            _ipAddress = ipAddress;
            _port = port;
            _listener = new TcpListener(_ipAddress, _port);
            _commandHandler = commandHandler;
            _logger = logger;
            _sessions = new Dictionary<int, PlayerSession>();
        }

        public bool IsRunning
        {
            get { return _isRunning; }
        }

        public async Task Start()
        {
            _listener.Start();
            _isRunning = true;
            _logger.Information("Server started on port " + _port);
            try
            {
                while (_isRunning)
                {
                    TcpClient client = await _listener.AcceptTcpClientAsync();
                    _logger.Information("Client connected");
                    var session = new PlayerSession(client, _commandHandler);
                    _sessions.Add(_sessionCounter++, session);
                    HandleClient(session).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Server encountered an error: " + ex.Message);
                _isRunning = false;
            }
            finally
            {
                _listener.Stop();
            }
        }

        private async Task HandleClient(PlayerSession session)
        {
            TcpClient client = session.Client;
            try
            {
                var stream = client.GetStream();
                var buffer = new byte[1024];
                int bytesRead;

                while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                {
                    var message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    session.ProcessInput(message.Trim());
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error handling client: " + ex.Message);
            }
            finally
            {
                client.Close();
                _sessions.Remove(_sessions.FirstOrDefault(x => x.Value == session).Key);
                _logger.Information("Client disconnected");
            }
        }

        public void Stop()
        {
            _isRunning = false;
            _listener.Stop();
            _logger.Information("Server stopped");
        }
    }
}
