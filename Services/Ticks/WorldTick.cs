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
            // Log world tick occurrence
            _logger.Information($"World tick occurred at {DateTime.Now}");

            // Implement world logic here
        }

        public TimeSpan GetInterval()
        {
            return TimeSpan.FromSeconds(45);
        }
    }
}
