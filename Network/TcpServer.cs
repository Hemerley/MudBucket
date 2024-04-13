using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using MudBucket.Interfaces;

namespace MudBucket.Network
{
    public class TcpServer
    {
        private readonly ILogger _logger;
        private readonly ICommandParser _commandParser;
        private TcpListener _listener;
        private readonly int _port;
        private readonly int _bufferSize;

        public TcpServer(IPAddress ipAddress, int port, int bufferSize, ILogger logger, ICommandParser commandParser)
        {
            _logger = logger;
            _commandParser = commandParser;
            _port = port;
            _bufferSize = bufferSize;
            _listener = new TcpListener(ipAddress, port);
        }

        public async Task StartAsync()
        {
            _listener.Start();
            _logger.Information("Server started on port " + _port);

            try
            {
                while (true)
                {
                    var client = await _listener.AcceptTcpClientAsync();
                    _logger.Information("Client connected");
                    _ = HandleClient(client);  // Handle client asynchronously
                }
            }
            finally
            {
                _listener.Stop();
                _logger.Information("Server stopped");
            }
        }

        private async Task HandleClient(TcpClient client)
        {
            try
            {
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[_bufferSize];
                int bytesRead;
                bool keepConnection = true;

                while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) != 0 && keepConnection)
                {
                    var message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    _logger.Debug("Received: " + message);
                    keepConnection = _commandParser.ParseCommand(message, client);
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
