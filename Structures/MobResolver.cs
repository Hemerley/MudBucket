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
                int roomId = mob.RoomId;
                Console.WriteLine($"Attempting to place mob {mob.Name} in room {roomId}");

                if (roomMap[roomId].Id == roomId)
                {
                    // Assuming you need to update the mob's CurrentRoom property to reference the actual Room object
                    mob.CurrentRoom = roomMap[roomId]; // This might need to be adjusted based on your class structure

                    Console.WriteLine($"Placed mob {mob.Name} in room {roomMap[roomId].Name}");

                    // Ensure the room has an initialized Mobs list
                    if (roomMap[roomId].Mobs == null)
                    {
                        roomMap[roomId].Mobs = new List<Mob>();
                    }
                    roomMap[roomId].Mobs.Add(mob);
                }
                else
                {
                    Console.WriteLine($"Room ID {roomId} not found for mob {mob.Name}");
                }
            }
        }
    }
}
