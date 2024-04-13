using MudBucket.Interfaces;

namespace MudBucket.Systems
{
    public class GameTickTimer : ITickTimer
    {
        private readonly IScheduler _scheduler;

        public GameTickTimer(IScheduler scheduler)
        {
            _scheduler = scheduler;
        }

        public void RegisterTickable(ITickable tickable)
        {
            _scheduler.ScheduleTickable(tickable);
        }

        public void UnregisterTickable(ITickable tickable)
        {
            _scheduler.UnscheduleTickable(tickable);
        }
    }
}
