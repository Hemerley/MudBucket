using MudBucket.Interfaces;
using MudBucket.Structures;

namespace MudBucket.Systems
{
    public class PromptService : IPromptService
    {
        public string GeneratePrompt(Player player)
        {
            return ReplaceTokens(player, player.PromptFormat);
        }

        public string GenerateBattlePrompt(Player player, Mob enemy)
        {
            return ReplaceTokensWithEnemy(player, enemy, player.BattlePromptFormat);
        }

        private string ReplaceTokens(Player player, string format)
        {
            return format.Replace("{health}", player.Health.ToString())
                         .Replace("{mana}", player.Mana.ToString())
                         .Replace("{maxHealth}", player.MaxHealth.ToString())
                         .Replace("{maxMana}", player.MaxMana.ToString())
                         .Replace("{moves}", player.Moves.ToString())
                         .Replace("{maxMoves}",player.MaxMoves.ToString());
        }

        private string ReplaceTokensWithEnemy(Player player, Mob enemy, string format)
        {
            return ReplaceTokens(player, format)
                   .Replace("{enemyHealth}", enemy.Health.ToString());
        }
    }
}
