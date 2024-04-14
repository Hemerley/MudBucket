using MudBucket.Interfaces;
using MudBucket.Systems;
namespace MudBucket.States
{
    public class InGameState : IPlayerState
    {
        public void ProcessInput(PlayerSession session, string input)
        {
            _ = session.ProcessInput(input);
        }
    }
}
