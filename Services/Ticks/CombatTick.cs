using MudBucket.Interfaces;
using System;

namespace MudBucket.Services.Ticks
{
    public class CombatTick : ITickable
    {
        private readonly ILogger _logger;

        public CombatTick(ILogger logger)
        {
            _logger = logger;
        }

        public void Tick()
        {
            _logger.Information("Combat tick occurred at " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            // Implement additional combat logic here
        }

        public TimeSpan GetInterval()
        {
            return TimeSpan.FromSeconds(1);  // Triggers every second
        }
    }
}
