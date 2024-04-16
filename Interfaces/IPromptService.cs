using MudBucket.Structures;

namespace MudBucket.Interfaces
{
    public interface IPromptService
    {
        string GeneratePrompt(Player player);
        string GenerateBattlePrompt(Player player, Mob enemy);
    }
}
