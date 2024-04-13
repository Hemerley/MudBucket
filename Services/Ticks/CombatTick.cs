using MudBucket.Interfaces;

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
            // Log combat tick occurrence
            _logger.Information($"Combat tick occurred at {DateTime.Now}");

            // Implement combat logic here
        }

        public TimeSpan GetInterval()
        {
            return TimeSpan.FromSeconds(1); // Specify the interval for combat ticks
        }
    }
}
