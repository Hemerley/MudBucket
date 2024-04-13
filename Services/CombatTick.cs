using MudBucket.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MudBucket.Services
{
    public class CombatTick : ITickable
    {
        public void Tick()
        {
            // Here you implement what happens during a combat tick
            Console.WriteLine("Combat tick occurred at " + DateTime.Now);
            // Example: Update combat states, check for cooldown expiration, AI decisions, etc.
        }
    }
}
