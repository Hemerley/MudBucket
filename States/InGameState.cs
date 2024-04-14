using MudBucket.Interfaces;
using MudBucket.Systems;
namespace MudBucket.States
{
    public class InGameState : IPlayerState
    {
        public void ProcessInput(PlayerSession session, string input)
        {
            // This example just echoes back the commands, but in a real application,
            // this would handle complex game interactions
            if (input.Equals("logout", StringComparison.OrdinalIgnoreCase))
            {
                session.SetState(new NewConnectionState()); // Resets to connection state for demonstration
                session.SendToClient("You have logged out. See you next time!", session.Client);
            }
            else
            {
                // Process game commands here
                session.SendToClient("You input: " + input, session.Client);
            }
        }
    }

}
