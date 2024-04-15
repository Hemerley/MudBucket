using MudBucket.Interfaces;
using MudBucket.Network;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MudBucket
{
    public class ServerManager
    {
        private readonly IScheduler _scheduler;
        private readonly TcpServer _server;
        private bool _isShutdownInitiated = false;
        private Task _shutdownTask;
        private CancellationTokenSource _cts;  // Cancellation token source for managing task cancellation

        public ServerManager(IScheduler scheduler, TcpServer server)
        {
            _scheduler = scheduler;
            _server = server;
        }

        public void StartServer()
        {
            _scheduler.Start();
            _server.Start();
            Console.WriteLine("Server and scheduler have started.");
        }

        public void StopServer()
        {
            _scheduler.Stop();
            _server.Stop();
            Console.WriteLine("Server and scheduler have stopped.");
        }

        public void InitiateShutdown()
        {
            if (!_isShutdownInitiated)
            {
                _isShutdownInitiated = true;
                _cts = new CancellationTokenSource();  // Initialize the CancellationTokenSource
                Console.WriteLine("Shutdown initiated. Server will shutdown in 5 minutes.");
                _shutdownTask = StartShutdownSequence(_cts.Token);  // Pass the token to the task
            }
        }

        public void AbortShutdown()
        {
            if (_isShutdownInitiated && _cts != null)
            {
                _cts.Cancel();  // Cancel the task using the CancellationToken
                _isShutdownInitiated = false;
                Console.WriteLine("Shutdown aborted.");
                _server.BroadcastMessage("[white][[server_warning]Warning[white]][server] The server shutdown has been aborted[white].");
            }
        }

        private async Task StartShutdownSequence(CancellationToken token)
        {
            int minutesLeft = 5;
            while (minutesLeft > 0 && !_cts.IsCancellationRequested)
            {
                _server.BroadcastMessage($"[white][[server_warning]Warning[white]][server] The server is shutting down in [white]{minutesLeft}[server] minutes[white]. [server]Please finish your activities and log off[white].");
                await Task.Delay(TimeSpan.FromMinutes(1), token);  // Respect cancellation
                if (token.IsCancellationRequested)
                    break;  // Break the loop if cancellation is requested
                minutesLeft--;
            }
            if (!_cts.IsCancellationRequested)
            {
                StopServer();
            }
        }
    }
}
