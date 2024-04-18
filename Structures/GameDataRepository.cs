using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace MudBucket.Structures
{
    public class GameDataRepository
    {
        private static GameDataRepository _instance;
        public static GameDataRepository Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameDataRepository();
                }
                return _instance;
            }
        }

        public List<PlayerClass> PlayerClasses { get; private set; }
        public List<Race> Races { get; private set; }
        public List<Mob> Mobs { get; private set; }
        public List<Room> Rooms { get; private set; }

        private GameDataRepository()
        {
            LoadData();
        }

        private void LoadData()
        {
            // Load player classes and races
            PlayerClasses = JsonConvert.DeserializeObject<List<PlayerClass>>(File.ReadAllText("classes.json"));
            Races = JsonConvert.DeserializeObject<List<Race>>(File.ReadAllText("races.json"));

            // Load rooms and then resolve their exits
            Rooms = RoomResolver.LoadRoomsFromFile("rooms.json");
            RoomResolver.ResolveRoomExits(Rooms);
            var roomMap = Rooms.ToDictionary(r => r.Id, r => r);

            // Load mobs and resolve them to rooms
            Mobs = MobResolver.LoadMobsFromFile("mobs.json");
            MobResolver.ResolveMobsToRooms(Mobs, roomMap);
        }
    }
}
