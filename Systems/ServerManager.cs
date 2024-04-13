using MudBucket.Interfaces;
using MudBucket.Network;
using MudBucket.Systems;
using System.Collections.Generic;

namespace MudBucket
{
    public class ServerManager
    {
        private readonly ITickTimer _tickTimer;
        private readonly TcpServer _server;
        private readonly List<ITickable> _tickables;  // List to manage tickable instances
        private readonly int _tickIntervalMs;

        public ServerManager(ITickTimer tickTimer, TcpServer server, int tickIntervalMs = 1000)
        {
            _tickTimer = tickTimer;
            _server = server;
            _tickIntervalMs = tickIntervalMs;
            _tickables = new List<ITickable>();  // Initialize the list of tickables
        }

        // Method to add tickable instances
        public void RegisterTickable(ITickable tickable)
        {
            if (!_tickables.Contains(tickable))
            {
                _tickables.Add(tickable);
                // If the server is already running, dynamically register tickable to timer
                if (_server.IsRunning)
                {
                    _tickTimer.RegisterTickable(tickable);
                }
            }
        }

        // Method to remove tickable instances
        public void UnregisterTickable(ITickable tickable)
        {
            if (_tickables.Contains(tickable))
            {
                _tickables.Remove(tickable);
                // If the server is still running, dynamically unregister tickable from timer
                if (_server.IsRunning)
                {
                    _tickTimer.UnregisterTickable(tickable);
                }
            }
        }

        public void StartServer()
        {
            // Register all tickables with the tick timer
            foreach (ITickable tickable in _tickables)
            {
                _tickTimer.RegisterTickable(tickable);
            }

            // Start the tick timer with the specified interval
            _tickTimer.StartTimer(_tickIntervalMs);

            // Start the TCP server
            _server.Start();
        }

        public void StopServer()
        {
            // Stop the tick timer and unregister all tickables
            _tickTimer.StopTimer();
            foreach (ITickable tickable in _tickables)
            {
                _tickTimer.UnregisterTickable(tickable);
            }

            // Stop the TCP server
            _server.Stop();
        }
    }
}
