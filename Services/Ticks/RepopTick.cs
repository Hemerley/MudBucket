using MudBucket.Interfaces;
using System;

namespace MudBucket.Services.Ticks
{
    public class RepopTick : ITickable
    {
        private readonly ILogger _logger;

        public RepopTick(ILogger logger)
        {
            _logger = logger;
        }

        public void Tick()
        {
            _logger.Information("Repop tick occurred at " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            // Implement logic for repopulating or refreshing game elements
        }

        public TimeSpan GetInterval()
        {
            return TimeSpan.FromMinutes(3);  // Triggers every 3 minutes
        }
    }
}
