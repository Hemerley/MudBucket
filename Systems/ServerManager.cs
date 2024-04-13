using MudBucket.Interfaces;
using MudBucket.Network;

namespace MudBucket
{
    public class ServerManager
    {
        private readonly IScheduler _scheduler;
        private readonly TcpServer _server;
        private readonly List<ITickable> _tickables;  // List to manage tickable instances

        public ServerManager(IScheduler scheduler, TcpServer server)
        {
            _scheduler = scheduler;
            _server = server;
            _tickables = new List<ITickable>();  // Initialize the list of tickables
        }

        // Method to add tickable instances
        public void RegisterTickable(ITickable tickable)
        {
            if (!_tickables.Contains(tickable))
            {
                _tickables.Add(tickable);
                // If the server is already running, dynamically register tickable to scheduler
                if (_server.IsRunning)
                {
                    _scheduler.ScheduleTickable(tickable);
                }
            }
        }

        // Method to remove tickable instances
        public void UnregisterTickable(ITickable tickable)
        {
            if (_tickables.Contains(tickable))
            {
                _tickables.Remove(tickable);
                // If the server is still running, dynamically unregister tickable from scheduler
                if (_server.IsRunning)
                {
                    _scheduler.UnscheduleTickable(tickable);
                }
            }
        }

        public void StartServer()
        {

            // Register all tickables with the scheduler
            foreach (ITickable tickable in _tickables)
            {
                _scheduler.ScheduleTickable(tickable);
            }

            // Start the TCP server
            _server.Start();
        }

        public void StopServer()
        {
            foreach (ITickable tickable in _tickables)
            {
                _scheduler.UnscheduleTickable(tickable);
            }

            // Stop the TCP server
            _server.Stop();
        }
    }
}
