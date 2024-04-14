using MudBucket.Interfaces;
using MudBucket.Systems;
namespace MudBucket.States
{
    public class NewPlayerRegistrationState : IPlayerState
    {
        public void ProcessInput(PlayerSession session, string input)
        {
            // Handle registration inputs here
            session.SetState(new CharacterCustomizationState());
            session.SendToClient("Registration successful. Let's customize your character...", session.Client);
        }
    }
}
