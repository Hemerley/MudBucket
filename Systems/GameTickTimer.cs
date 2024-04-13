using MudBucket.Interfaces;

namespace MudBucket.Systems
{
    public class GameTickTimer : ITickTimer
    {
        private Timer _timer;
        private readonly List<ITickable> _tickables = new List<ITickable>(); // List to manage tickable instances
        private readonly object _lock = new object(); // Lock for thread-safe operations

        public void RegisterTickable(ITickable tickable)
        {
            lock (_lock)
            {
                if (!_tickables.Contains(tickable))
                {
                    _tickables.Add(tickable);
                }
            }
        }

        public void UnregisterTickable(ITickable tickable)
        {
            lock (_lock)
            {
                if (_tickables.Contains(tickable))
                {
                    _tickables.Remove(tickable);
                }
            }
        }

        public void StartTimer(int intervalMs)
        {
            _timer = new Timer(Tick, null, 0, intervalMs);
        }

        public void StopTimer()
        {
            _timer?.Change(Timeout.Infinite, 0);
            _timer?.Dispose();
        }

        private void Tick(object state)
        {
            lock (_lock)
            {
                foreach (ITickable tickable in _tickables)
                {
                    try
                    {
                        tickable.Tick();
                    }
                    catch (Exception ex)
                    {
                        // Log the error or handle it appropriately
                        // This prevents one failing tickable from interrupting others
                        Console.WriteLine($"Error during tick operation: {ex.Message}");
                    }
                }
            }
        }
    }
}
