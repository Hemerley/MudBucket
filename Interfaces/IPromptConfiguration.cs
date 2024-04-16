using MudBucket.Structures;

namespace MudBucket.Interfaces
{
    public interface IPromptConfiguration
    {
        string GetPromptFormat(Player player);
        string GetBattlePromptFormat(Player player);
        void SetPromptFormat(Player player, string format);
        void SetBattlePromptFormat(Player player, string format);
    }
}
