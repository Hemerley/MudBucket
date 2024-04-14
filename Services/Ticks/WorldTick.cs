using MudBucket.Interfaces;

namespace MudBucket.Services.Ticks
{
    public class WorldTick : ITickable
    {
        private readonly ILogger _logger;

        public WorldTick(ILogger logger)
        {
            _logger = logger;
        }

        public void Tick()
        {
            _logger.Information("World tick occurred at " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            // Here, you would handle world updates such as day/night cycles
        }

        public TimeSpan GetInterval()
        {
            return TimeSpan.FromSeconds(45);  // Triggers every 45 seconds
        }
    }
}
