using MudBucket.Interfaces;

namespace MudBucket.Systems
{
    public interface ITickTimer
    {
        void StartTimer(int intervalMs);
        void StopTimer();
        void RegisterTickable(ITickable tickable);
        void UnregisterTickable(ITickable tickable);
    }
}
