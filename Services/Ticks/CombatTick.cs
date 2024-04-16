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
            _logger.Information("Combat tick occurred at " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }
        public TimeSpan GetInterval()
        {
            return TimeSpan.FromSeconds(1);
        }
    }
}
