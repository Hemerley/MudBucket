using MudBucket.Interfaces;

namespace MudBucket.Services.Ticks
{
    public class WorldTick : ITickable
    {
        public void Tick()
        {
            Console.WriteLine("World tick occurred at " + DateTime.Now);
        }
    }
}
