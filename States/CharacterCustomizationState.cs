using MudBucket.Interfaces;
using MudBucket.Systems;

namespace MudBucket.States
{
    public class CharacterCustomizationState : IPlayerState
    {
        public void ProcessInput(PlayerSession session, string input)
        {
            _ = session.ProcessInput(input);
        }
    }
}
