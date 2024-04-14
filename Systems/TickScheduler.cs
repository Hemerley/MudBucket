using MudBucket.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MudBucket.Systems
{
    public class TickScheduler : IScheduler
    {
        private readonly Dictionary<ITickable, TimeSpan> _scheduledTickables = new Dictionary<ITickable, TimeSpan>();
        private readonly object _lock = new object();
        private Task _schedulerTask;
        private bool _isRunning;

        public void Start()
        {
            if (!_isRunning)
            {
                _isRunning = true;
                _schedulerTask = Task.Run(SchedulerLoop);
            }
        }

        public void Stop()
        {
            _isRunning = false;
            _schedulerTask?.Wait();
        }

        public void ScheduleTickable(ITickable tickable)
        {
            lock (_lock)
            {
                _scheduledTickables[tickable] = tickable.GetInterval();
            }
        }

        public void UnscheduleTickable(ITickable tickable)
        {
            lock (_lock)
            {
                _scheduledTickables.Remove(tickable);
            }
        }

        private async Task SchedulerLoop()
        {
            while (_isRunning)
            {
                await Task.Delay(TimeSpan.FromSeconds(1)); // Adjust as needed for tick granularity

                lock (_lock)
                {
                    foreach (var tickable in new List<ITickable>(_scheduledTickables.Keys))
                    {
                        var interval = _scheduledTickables[tickable];
                        if (interval <= TimeSpan.Zero)
                        {
                            tickable.Tick();
                            _scheduledTickables[tickable] = tickable.GetInterval(); // Reset interval
                        }
                        else
                        {
                            _scheduledTickables[tickable] -= TimeSpan.FromSeconds(1);
                        }
                    }
                }
            }
        }
    }
}
