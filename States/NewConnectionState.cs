using MudBucket.Interfaces;
using MudBucket.Systems;

namespace MudBucket.States
{
    public class NewConnectionState : IPlayerState
    {
        public void ProcessInput(PlayerSession session, string input)
        {
            if (input.Equals("new", StringComparison.OrdinalIgnoreCase))
            {
                session.SetState(new NewPlayerRegistrationState());
                session.SendToClient("Please choose a username:", session.Client);
            }
            else if (input.Equals("returning", StringComparison.OrdinalIgnoreCase))
            {
                session.SetState(new ReturningPlayerLoginState());
                session.SendToClient("Please enter your username:", session.Client);
            }
            else
            {
                session.SendToClient("Please type 'new' for new player or 'returning' for returning player.", session.Client);
            }
        }
    }
}
