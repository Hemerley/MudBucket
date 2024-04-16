using MudBucket.Interfaces;

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
        }
        public TimeSpan GetInterval()
        {
            return TimeSpan.FromMinutes(3);
        }
    }
}
