namespace MudBucket.Interfaces
{
    public interface ITickable
    {
        void Tick();
        TimeSpan GetInterval();
    }
}
