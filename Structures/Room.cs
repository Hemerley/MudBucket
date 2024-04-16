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
        public Dictionary<string, Room> Exits { get; set; }
        public List<Quest> Quests { get; set; }
        public bool IsPersistent { get; set; }
        public SectorType SectorType { get; set; }
        public List<RoomFlag> Flags { get; set; }
        public Room()
        {
            Items = new List<Item>();
            Mobs = new List<Mob>();
            Exits = new Dictionary<string, Room>();
            Quests = new List<Quest>();
            IsPersistent = false;
            Flags = new List<RoomFlag>();
        }
    }
}