namespace MudBucket.Interfaces
{
    public interface IScheduler
    {
        void ScheduleTickable(ITickable tickable);
        void UnscheduleTickable(ITickable tickable);
        void Start();
        void Stop();
    }
}
