using Newtonsoft.Json;
using System.Collections.Generic;

namespace MudBucket.Structures
{
    public enum MobType
    {
        NPC,
        Monster,
        Vendor,
        QuestGiver
    }

    public class Mob
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Health { get; set; }
        public MobType Type { get; set; }
        public Dictionary<string, int> Stats { get; set; } = new Dictionary<string, int>
        {
            {"Attack", 5},
            {"Defense", 5}
        };
        public int RoomId { get; set; }  // ID of the room the mob is initially located in

        [JsonIgnore]
        public Room CurrentRoom { get; set; }  // Actual room object, resolved after deserialization

        // Constructor
        public Mob()
        {
            // Default Stats are initialized here, can be overridden later
        }

    }
}
