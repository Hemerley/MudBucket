using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using MudBucket.Interfaces;

public class TcpServer
{
    private readonly IPAddress ipAddress;
    private readonly int port;
    private readonly int bufferSize;
    private readonly int connectionTimeout;  // In seconds
    private readonly ILogger logger;
    private TcpListener listener;

    public TcpServer(IPAddress ipAddress, int port, int bufferSize, int connectionTimeout, ILogger logger)
    {
        this.ipAddress = ipAddress;
        this.port = port;
        this.bufferSize = bufferSize;
        this.connectionTimeout = connectionTimeout;
        this.logger = logger;
    }

    public async Task StartAsync()
    {
        listener = new TcpListener(ipAddress, port);
        listener.Start();
        logger.Information($"Server started on {ipAddress}:{port}. Listening for connections...");

        try
        {
            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
                logger.Information("Client connected.");
                HandleClientAsync(client);
            }
        }
        catch (Exception ex)
        {
            logger.Error($"Error in server main loop: {ex.Message}");
        }
        finally
        {
            listener.Stop();
        }
    }

    private async void HandleClientAsync(TcpClient client)
    {
        try
        {
            using (client)
            {
                client.ReceiveTimeout = connectionTimeout * 1000;  // Convert seconds to milliseconds
                var stream = client.GetStream();
                var buffer = new byte[bufferSize];
                int bytesRead;

                while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                {
                    string receivedText = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    logger.Debug($"Received: {receivedText}");

                    // Echo the data back to the client
                    await stream.WriteAsync(buffer, 0, bytesRead);
                    logger.Debug($"Sent: {receivedText}");
                }
            }
        }
        catch (Exception ex)
        {
            if (ex is SocketException socketEx && socketEx.SocketErrorCode == SocketError.TimedOut)
            {
                logger.Error("Connection timed out.");
            }
            else
            {
                logger.Error($"Client handling error: {ex.Message}");
            }
        }
        finally
        {
            if (client.Connected)
            {
                client.Close();
            }
            logger.Information("Client disconnected.");
        }
    }
}
