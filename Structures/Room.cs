namespace MudBucket.Structures
{
    public class Room
    {
        public string Description { get; set; }
        public List<Item> Items { get; set; }
        public List<Mob> Mobs { get; set; }
        public Dictionary<string, Room> Exits { get; set; }
        public List<Quest> Quests { get; set; }
        public bool IsPersistent { get; set; }

        public Room()
        {
            Items = new List<Item>();
            Mobs = new List<Mob>();
            Exits = new Dictionary<string, Room>();
            Quests = new List<Quest>();
            IsPersistent = false;
        }
    }
}
