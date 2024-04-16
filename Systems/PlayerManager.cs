using MudBucket.Structures;
using System.Net.Sockets;

namespace MudBucket.Systems
{
    public class PlayerManager
    {
        private readonly List<PlayerClass> _characterClasses;
        private readonly List<Race> _races;
        public PlayerManager(List<PlayerClass> characterClasses, List<Race> races)
        {
            _characterClasses = characterClasses;
            _races = races;
        }
        public Player CreatePlayer(TcpClient client, string name, string className, string raceName)
        {
            var playerClass = _characterClasses.FirstOrDefault(c => c.Name == className);
            var race = _races.FirstOrDefault(r => r.Name == raceName);

            if (playerClass == null || race == null)
                throw new ArgumentException("Invalid class or race name.");

            var player = new Player(client)
            {
                Name = name,
                PlayerClass = className,
                Race = raceName,
                Level = 1,
                Health = 100,
                Mana = 50,
                MaxHealth = 100,
                MaxMana = 50
            };
            foreach (var attr in race.BaseAttributes)
            {
                player.Attributes[attr.Key] = attr.Value;
            }

            return player;
        }
        public void LevelUpPlayer(Player player)
        {
            player.Level++;
            player.MaxHealth += 20;
            player.MaxMana += 15;
            player.Health = player.MaxHealth;
            player.Mana = player.MaxMana;
            player.SendMessage($"Congratulations! You've leveled up to {player.Level}.");
        }
    }
}