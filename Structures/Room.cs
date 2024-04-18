using MudBucket.Systems;
using Newtonsoft.Json;
using System.Text;

namespace MudBucket.Structures
{
    public enum SectorType
    {
        Inside,
        City,
        Field,
        Forest,
        Hills,
        Mountains,
        WaterSwim,
        WaterNoSwim,
        Underwater,
        Air,
        Desert,
        Dungeon,
        Road,
        Unused,
        UnderwaterNoSwim,
        Swamp,
        Beach,
        Volcano,
        OOC,
        Count
    }

    public enum RoomFlag
    {
        Dark,
        DeathTrap,
        NoMob,
        Indoors,
        Peaceful,
        Soundproof,
        NoMagic,
        Tunnel,
        Private,
        Safe,
        NoRecall,
        NoTeleport,
        HailOnly,
        Arena,
        Lock,
        NoScan,
        NoTeleportOut,
        Hotel,
        Store,
        Bank,
        PetShop,
        PostOffice,
        NoFlee,
        NoView,
        Prototype,
        NoAstral,
        NoSummon,
        NoTrack,
        NoGate,
        NoWeb,
        NoDominion,
        NoWorld,
        Silent,
        NoManapool,
        NoResting,
        Solitary,
        NoSteal,
        NoRemote,
        NoEdit,
        Count
    }

    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Item> Items { get; set; } = new List<Item>(); // Placeholder for actual item types
        public List<Mob> Mobs { get; set; } = new List<Mob>();  // Placeholder for actual mob types
        public Dictionary<string, int> Exits { get; set; } = new Dictionary<string, int>(); // Room ID references
        public List<Quest> Quests { get; set; } = new List<Quest>(); // Placeholder for actual quest types
        public bool IsPersistent { get; set; }
        public SectorType SectorType { get; set; }
        public List<RoomFlag> Flags { get; set; } = new List<RoomFlag>();

        [JsonIgnore]
        public Dictionary<string, Room> ResolvedExits { get; set; } = new Dictionary<string, Room>();

        public Room()
        {
            Items = new List<Item>();
            Mobs = new List<Mob>();
            Exits = new Dictionary<string, int>();
            Quests = new List<Quest>();
            Flags = new List<RoomFlag>();
        }

        public void ResolveExits(Dictionary<int, Room> roomMap)
        {
            foreach (var exit in Exits)
            {
                if (roomMap.TryGetValue(exit.Value, out Room linkedRoom))
                {
                    ResolvedExits.Add(exit.Key, linkedRoom);
                }
            }
        }

        public void DescribeRoom(Player player)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"[white]{Name}");  // Room name in bold and white color
            stringBuilder.AppendLine($"{Description}");  // Room description

            if (Items.Any())
            {
                stringBuilder.AppendLine(); // Added space
                stringBuilder.AppendLine("[yellow]You see here:");
                foreach (var item in Items)
                {
                    stringBuilder.AppendLine($"[yellow]- [cyan]{item.Name}");  // Item names in cyan color
                }
            }

            if (Mobs.Any())
            {
                stringBuilder.AppendLine(); // Added space
                stringBuilder.AppendLine("[red]Creatures:");
                foreach (var mob in Mobs)
                {
                    stringBuilder.AppendLine($"[red]- [green]{mob.Name}");  // Mob names in green color
                }
            }

            if (Exits.Any())
            {
                stringBuilder.AppendLine(); // Added space
                stringBuilder.AppendLine("[magenta]Exits:");
                foreach (var exit in Exits.Keys)
                {
                    stringBuilder.AppendLine($"[magenta]- [blue]{exit}");  // Exit directions in blue color
                }
            }
            player.SendMessage(stringBuilder.ToString());
        }
    }
}
