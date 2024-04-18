using Newtonsoft.Json;

namespace MudBucket.Structures
{
    public static class MobResolver
    {
        public static List<Mob> LoadMobsFromFile(string filePath)
        {
            var json = File.ReadAllText(filePath);
            var mobs = JsonConvert.DeserializeObject<List<Mob>>(json);
            return mobs;
        }

        public static void ResolveMobsToRooms(List<Mob> mobs, Dictionary<int, Room> roomMap)
        {
            foreach (var mob in mobs)
            {
                if (roomMap.TryGetValue(mob.RoomId, out Room room))
                {
                    mob.CurrentRoom = room;
                    // Optionally, add the mob to the room's Mobs list if it exists
                    if (room.Mobs is List<Mob> roomMobs)
                    {
                        roomMobs.Add(mob);
                    }
                    else
                    {
                        room.Mobs = new List<Mob> { mob };
                    }
                }
            }
        }
    }
}
