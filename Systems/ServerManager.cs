using MudBucket.Interfaces;
using MudBucket.Network;

namespace MudBucket
{
    public class ServerManager
    {
        private readonly IScheduler _scheduler;
        private readonly TcpServer _server;
        private readonly List<ITickable> _tickables;

        public ServerManager(IScheduler scheduler, TcpServer server)
        {
            _scheduler = scheduler;
            _server = server;
            _tickables = new List<ITickable>();
        }

        public void RegisterTickable(ITickable tickable)
        {
            if (!_tickables.Contains(tickable))
            {
                _tickables.Add(tickable);
                if (_server.IsRunning)
                {
                    _scheduler.ScheduleTickable(tickable);
                }
            }
        }

        public void UnregisterTickable(ITickable tickable)
        {
            if (_tickables.Contains(tickable))
            {
                _tickables.Remove(tickable);
                if (_server.IsRunning)
                {
                    _scheduler.UnscheduleTickable(tickable);
                }
            }
        }

        public void StartServer()
        {
            foreach (ITickable tickable in _tickables)
            {
                _scheduler.ScheduleTickable(tickable);
            }
            _ = _server.Start();
        }

        public void StopServer()
        {
            foreach (ITickable tickable in _tickables)
            {
                _scheduler.UnscheduleTickable(tickable);
            }
            _server.Stop();
        }
    }
}
