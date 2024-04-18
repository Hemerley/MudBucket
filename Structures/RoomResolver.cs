using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MudBucket.Structures
{
    public static class RoomResolver
    {
        public static List<Room> LoadRoomsFromFile(string filePath)
        {
            var json = File.ReadAllText(filePath);
            var rooms = JsonConvert.DeserializeObject<List<Room>>(json);
            return rooms;
        }

        public static void ResolveRoomExits(List<Room> rooms)
        {
            var roomMap = rooms.ToDictionary(r => r.Id, r => r);

            foreach (var room in rooms)
            {
                foreach (var exit in room.Exits)
                {
                    if (roomMap.TryGetValue(exit.Value, out Room linkedRoom))
                    {
                        room.ResolvedExits.Add(exit.Key, linkedRoom);
                    }
                }
            }
        }
    }
}
