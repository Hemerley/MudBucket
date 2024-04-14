using MudBucket.Interfaces;
using MudBucket.Services.Commands;
using MudBucket.Systems;
namespace MudBucket.States
{
    public class InGameState : IPlayerState
    {
        public void ProcessInput(PlayerSession session, string input)
        {
            session.ProcessInput(input);
        }
    }
}
