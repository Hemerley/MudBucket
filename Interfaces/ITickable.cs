namespace MudBucket.Interfaces
{
    public interface ITickable
    {
        void Tick();
        TimeSpan GetInterval(); // New method to specify the tick interval
    }
}
