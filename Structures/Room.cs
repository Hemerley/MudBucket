using Microsoft.Extensions.Primitives;
using MudBucket.Interfaces;
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
        public List<Item> Items { get; set; }
        public List<Mob> Mobs { get; set; }
        public Dictionary<string, int> Exits { get; set; } = new Dictionary<string, int>();
        public List<Quest> Quests { get; set; }
        public List<Player> Players { get; set; }
        public bool IsPersistent { get; set; }
        public SectorType SectorType { get; set; }
        public List<RoomFlag> Flags { get; set; }

        [JsonIgnore]
        public Dictionary<string, Room> ResolvedExits { get; set; } = new Dictionary<string, Room>();

        private AnsiColorManager _colorManager = new AnsiColorManager();

        public Room()
        {
            Players = new List<Player>();
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
            stringBuilder.AppendLine(_colorManager.ApplyColorCodes("[bright_yellow]" + Name + "[reset]"));
            stringBuilder.AppendLine(_colorManager.ApplyColorCodes("[white]" + Description + "[reset]"));

            if (Items.Any())
            {
                stringBuilder.AppendLine();
                stringBuilder.AppendLine(_colorManager.ApplyColorCodes("[bright_cyan]You see here:[reset]"));
                foreach (var item in Items)
                {
                    stringBuilder.AppendLine(_colorManager.ApplyColorCodes("[bright_cyan]- " + item.Name + "[reset]"));
                }
            }

            if (Mobs.Any())
            {
                stringBuilder.AppendLine();
                stringBuilder.AppendLine(_colorManager.ApplyColorCodes("[bright_red]Creatures:[reset]"));
                foreach (var mob in Mobs)
                {
                    stringBuilder.AppendLine(_colorManager.ApplyColorCodes("[bright_red]- " + mob.Name + "[reset]"));
                }
            }

            if (Players.Any())
            {
                stringBuilder.AppendLine();
                stringBuilder.AppendLine(_colorManager.ApplyColorCodes("[bright_green]Players:[reset]"));
                foreach (var _player in Players)
                {
                    if (_player.Name != player.Name)
                    {
                        stringBuilder.AppendLine(_colorManager.ApplyColorCodes("[bright_green]- " + _player.Name + "[reset]"));
                    }
                }
            }

            if (Exits.Any())
            {
                stringBuilder.AppendLine();
                stringBuilder.AppendLine(_colorManager.ApplyColorCodes("[bright_magenta]Exits:[reset]"));
                foreach (var exit in Exits.Keys)
                {
                    stringBuilder.AppendLine(_colorManager.ApplyColorCodes("[bright_magenta]- " + exit + "[reset]"));
                }
            }
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(_colorManager.ApplyColorCodes("[server_info]Room ID: " + Id + "[reset]"));
            stringBuilder.AppendLine();
            player.SendMessage(stringBuilder.ToString());
        }

        public string GenerateAsciiMap(Dictionary<int, Room> roomMap)
        {
            StringBuilder mapBuilder = new StringBuilder();

            // Retrieve adjacent rooms
            Room northRoom, southRoom, eastRoom, westRoom;
            ResolvedExits.TryGetValue("north", out northRoom);
            ResolvedExits.TryGetValue("south", out southRoom);
            ResolvedExits.TryGetValue("east", out eastRoom);
            ResolvedExits.TryGetValue("west", out westRoom);

            // Prepare symbols for each room
            string northSymbol = northRoom != null ? GetRoomSymbol(northRoom.SectorType) : "X ";
            string southSymbol = southRoom != null ? GetRoomSymbol(southRoom.SectorType) : "X ";
            string eastSymbol = eastRoom != null ? GetRoomSymbol(eastRoom.SectorType) : "X ";
            string westSymbol = westRoom != null ? GetRoomSymbol(westRoom.SectorType) : "X ";
            string currentRoomSymbol = GetRoomSymbol(SectorType);

            // Map construction with squares and open paths ensuring all rows are aligned
            mapBuilder.AppendLine("    +---+       ");
            mapBuilder.AppendLine("    | " + northSymbol + "|       ");
            mapBuilder.AppendLine("    +---+       ");
            mapBuilder.AppendLine("+---+---+---+");
            mapBuilder.AppendLine("| " + westSymbol + "| " + currentRoomSymbol + "| " + eastSymbol + "|");
            mapBuilder.AppendLine("+---+---+---+");
            mapBuilder.AppendLine("    +---+       ");
            mapBuilder.AppendLine("    | " + southSymbol + "|       ");
            mapBuilder.AppendLine("    +---+       ");

            return mapBuilder.ToString();
        }


        private string GetRoomSymbol(SectorType type)
        {
            switch (type)
            {
                case SectorType.City:
                    return _colorManager.ApplyColorCodes("[bright_yellow]# [reset]");  // '#' can be visualized as clustered buildings of a city
                case SectorType.Forest:
                    return _colorManager.ApplyColorCodes("[green]* [reset]");  // '*' resembles trees or foliage
                case SectorType.WaterSwim:
                case SectorType.WaterNoSwim:
                    return _colorManager.ApplyColorCodes("[blue]~ [reset]");  // '~' often used to represent waves
                case SectorType.Mountains:
                    return _colorManager.ApplyColorCodes("[bright_black]^ [reset]");  // '^' is a common symbol for mountain peaks
                case SectorType.Desert:
                    return _colorManager.ApplyColorCodes("[yellow]= [reset]");  // '=' could be seen as layers of sand dunes
                default:
                    return _colorManager.ApplyColorCodes("[white]. [reset]");  // '.' for generic or unknown terrain types
            }
        }

    }
}
