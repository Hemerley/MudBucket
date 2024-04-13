using MudBucket.Interfaces;
using MudBucket.Services.Commands;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MudBucket.Network
{
    public class TcpServer
    {
        private readonly ILogger _logger;
        private readonly CommandHandler _commandHandler;
        private TcpListener _listener;
        private readonly int _port;
        private bool _isRunning;

        public bool IsRunning => _isRunning;  // Property to check if the server is currently running

        public TcpServer(IPAddress ipAddress, int port, ILogger logger, ICommandParser commandParser)
        {
            _logger = logger;
            _commandHandler = new CommandHandler(commandParser, logger);
            _port = port;
            _listener = new TcpListener(ipAddress, port);
        }

        public void Start()
        {
            _listener.Start();
            _isRunning = true;
            _logger.Information("Server started on port " + _port);
            Task.Run(() => AcceptClientsAsync());
        }

        public void Stop()
        {
            _isRunning = false;
            _listener.Stop();
            _logger.Information("Server stopped");
        }

        private async Task AcceptClientsAsync()
        {
            try
            {
                while (_isRunning)
                {
                    var client = await _listener.AcceptTcpClientAsync();
                    _logger.Information("Client connected");
                    HandleClientAsync(client).ContinueWith(t =>
                    {
                        if (t.Exception != null)
                            _logger.Error("Error handling client: " + t.Exception.Flatten().InnerException.Message);
                    }, TaskContinuationOptions.OnlyOnFaulted);
                }
            }
            catch (SocketException ex)
            {
                if (_isRunning)
                    _logger.Error("Listener stopped or an error occurred: " + ex.Message);
            }
        }

        private async Task HandleClientAsync(TcpClient client)
        {
            try
            {
                using (NetworkStream stream = client.GetStream())
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead;

                    while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                    {
                        var message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                        _logger.Debug("Received: " + message);
                        bool keepConnection = await _commandHandler.HandleCommandAsync(message, client);
                        if (!keepConnection) break;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("An error occurred with a client: " + ex.Message);
            }
            finally
            {
                if (client.Connected)
                {
                    client.Close();
                }
                _logger.Information("Client disconnected");
            }
        }
    }
}
