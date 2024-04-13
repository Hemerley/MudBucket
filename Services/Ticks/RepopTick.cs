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
            // Log repop tick occurrence
            _logger.Information($"Repop tick occurred at {DateTime.Now}");

            // Implement repop logic here
        }

        public TimeSpan GetInterval()
        {
            // Adjust the interval as needed
            return TimeSpan.FromMinutes(3);
        }
    }
}
