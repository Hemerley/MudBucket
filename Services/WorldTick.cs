using MudBucket.Interfaces;

namespace MudBucket.Services
{
    public class WorldTick : ITickable
    {
        public void Tick()
        {
            Console.WriteLine("World tick occurred at " + DateTime.Now);
        }
    }
}
