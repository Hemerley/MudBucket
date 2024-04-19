using MudBucket.Systems;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;
using MudBucket.Interfaces;
using MudBucket.Structures;
using Newtonsoft.Json;

namespace MudBucket.Commands
{
    public class NewCommand : CommandBase
    {
        private readonly AnsiColorManager _colorManager = new AnsiColorManager();

        public override SessionState[] ValidStates => new[] { SessionState.JustConnected };

        private Player player;

        protected override async Task<bool> ExecuteCommand(TcpClient client, INetworkService networkService, PlayerSession session)
        {
            await networkService.SendAsync("[white][[server_info]INFO[white]][server]Creating a new character...", session.player);

            string name = await RequestValidName(networkService);
            if (string.IsNullOrEmpty(name))
                return false;

            string password = await RequestValidPassword(networkService);
            if (string.IsNullOrEmpty(password))
                return false;

            string confirmPassword = await RequestConfirmation(networkService, "Confirm your password:");

            if (password != confirmPassword)
            {
                await networkService.SendAsync("[white][[server_info]ERROR[white]][server]Passwords do not match. Please try again.", session.player);
                return false;
            }

            string encryptedPassword = CryptoUtils.EncryptString(password);

            Race selectedRace = await SelectRace(networkService);
            await DisplayRaceAttributes(networkService, selectedRace);

            string playerClass = await SelectOption(networkService, Program.ServiceProvider.GetRequiredService<GameDataRepository>().PlayerClasses.Select(c => c.Name).ToList(), "Select your class:");

            player = new Player(session)
            {
                Name = name,
                Password = encryptedPassword,
                Race = selectedRace.Name,
                PlayerClass = playerClass,
                Level = 1,
                Health = 100,
                MaxHealth = 100,
                Mana = 100,
                MaxMana = 100,
                Moves = 100,
                MaxMoves = 100,
                Gold = 0,
                TriviaPoints = 0,
                QuestPoints = 0,
                Experience = 0,
                CampaignPoints = 0,
                Tier = 1,
                HeroStatus = false,
                Remort = 0,
                CurrentRoom = Program.ServiceProvider.GetRequiredService<GameDataRepository>().Rooms.FirstOrDefault(r => r.Id == 0),
                Attributes = new Dictionary<string, int>(selectedRace.BaseAttributes),
                Inventory = new List<Item>(),
                Equipment = new Dictionary<Player.EquipmentSlot, Item>(),
                Quests = new List<Quest>(),
                PromptFormat = "[bright_red]HP: {health}/{maxHealth} [bright_blue]Mana: {mana}/{maxMana} [bright_magenta]Moves: {moves}/{maxMoves}",
                BattlePromptFormat = "[bright_red]Battle HP: {health}/{maxHealth} [bright_blue]Mana: {mana}/{maxMana} [bright_magenta]Enemy HP: {enemyHealth}"
            };

            SaveCharacter(player);
            await networkService.SendAsync("[white][[server_info]INFO[white]][server]Character created successfully!", session.player);
            session.ChangeStateToPlaying();
            session.player = player;
            player.CurrentRoom.DescribeRoom(player);
            Program.ServiceProvider.GetRequiredService<GameDataRepository>().Rooms[player.CurrentRoom.Id].Players.Add(player);
            return true;
        }

        private async Task<string> RequestValidName(INetworkService networkService)
        {
            string name;
            Regex validNameRegex = new Regex("^[a-zA-Z]+$");
            do
            {
                await networkService.SendAsync("[white][[server_info]INFO[white]][server]Enter a name (letters only, no spaces):", player);
                name = await ReceiveAndSanitizeInput(networkService);
                if (!validNameRegex.IsMatch(name) || NameExists(name))
                {
                    await networkService.SendAsync("[white][[server_info]ERROR[white]][server]Invalid or taken name. Please try again.", player);
                    name = null;
                }
            } while (name == null);
            return name;
        }

        private async Task<string> RequestValidPassword(INetworkService networkService)
        {
            string password;
            Regex validPasswordRegex = new Regex("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^\\da-zA-Z]).{8,}$");
            do
            {
                await networkService.SendAsync("[white][[server_info]INFO[white]][server]Enter a password (at least 8 characters, must include uppercase, lowercase, numbers, and special characters):", player);
                password = await ReceiveAndSanitizeInput(networkService);
                if (!validPasswordRegex.IsMatch(password))
                {
                    await networkService.SendAsync("[white][[server_info]ERROR[white]][server]Password does not meet requirements. Please try again.", player);
                    password = null;
                }
            } while (password == null);
            return password;
        }

        private async Task<string> RequestConfirmation(INetworkService networkService, string prompt)
        {
            string confirmation;
            do
            {
                await networkService.SendAsync("[white][[server_info]INFO[white]][server]" + prompt, player);
                confirmation = await ReceiveAndSanitizeInput(networkService);
            } while (string.IsNullOrEmpty(confirmation));
            return confirmation;
        }

        private bool NameExists(string name)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Characters", $"{name}.json");
            return File.Exists(filePath);
        }

        private async Task<Race> SelectRace(INetworkService networkService)
        {
            string raceName = await SelectOption(networkService, Program.ServiceProvider.GetRequiredService<GameDataRepository>().Races.Select(r => r.Name).ToList(), "Select your race:");
            return GameDataRepository.Instance.Races.FirstOrDefault(r => r.Name.Equals(raceName, StringComparison.OrdinalIgnoreCase));
        }

        private async Task<string> SelectOption(INetworkService networkService, List<string> options, string prompt)
        {
            string selection;
            do
            {
                await networkService.SendAsync("[white][[server_info]INFO[white]][server]" + prompt + " " + string.Join(", ", options), player);
                selection = await ReceiveAndSanitizeInput(networkService);
                if (!options.Contains(selection, StringComparer.OrdinalIgnoreCase))
                {
                    await networkService.SendAsync("[white][[server_info]ERROR[white]][server]Invalid selection. Please try again.", player);
                    selection = null;
                }
            } while (selection == null);
            return selection;
        }

        private async Task DisplayRaceAttributes(INetworkService networkService, Race race)
        {
            StringBuilder attributesDisplay = new StringBuilder("[white][[server_info]INFO[white]][server]Your selected race's base attributes are:\n");
            foreach (var attribute in race.BaseAttributes)
            {
                attributesDisplay.AppendLine($"{attribute.Key}: {attribute.Value}");
            }
            await networkService.SendAsync(attributesDisplay.ToString(), player);
        }

        private void SaveCharacter(Player player)
        {
            string filePath = $"Characters/{player.Name}.json";
            string json = JsonConvert.SerializeObject(player, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            File.WriteAllText(filePath, json);
        }

        private async Task<string> ReceiveAndSanitizeInput(INetworkService networkService)
        {
            string input = await networkService.ReceiveAsync();
            input = input.Trim();
            input = input.TrimEnd('\r', '\n');
            return input;
        }
    }
}
