namespace MudBucket.Interfaces
{
    public interface ITickTimer
    {
        void RegisterTickable(ITickable tickable);
        void UnregisterTickable(ITickable tickable);
    }
}
