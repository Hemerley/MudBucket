using MudBucket.Interfaces;
using MudBucket.Systems;

namespace MudBucket.States
{
    public class ReturningPlayerLoginState : IPlayerState
    {
        public void ProcessInput(PlayerSession session, string input)
        {
            // Verify credentials and load player data
            session.SetState(new InGameState());
            session.SendToClient("Login successful. Welcome back to the game!", session.Client);
        }
    }
}
