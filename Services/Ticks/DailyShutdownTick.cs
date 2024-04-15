using MudBucket.Interfaces;
using System;

namespace MudBucket.Services.Ticks
{
    public class DailyShutdownTick : ITickable
    {
        private readonly ServerManager _serverManager;
        private DateTime _nextScheduledTime;

        public DailyShutdownTick(ServerManager serverManager)
        {
            _serverManager = serverManager;
            SetNextScheduledTime();
        }

        public void Tick()
        {
            if (DateTime.Now >= _nextScheduledTime)
            {
                _serverManager.InitiateShutdown();
                SetNextScheduledTime();
            }
        }

        public TimeSpan GetInterval()
        {
            return TimeSpan.FromMinutes(1); // Check every minute
        }

        private void SetNextScheduledTime()
        {
            var now = DateTime.Now;
            var today2AM = now.Date.AddHours(2); // 2 AM today
            _nextScheduledTime = now <= today2AM ? today2AM : today2AM.AddDays(1);
        }
    }
}
