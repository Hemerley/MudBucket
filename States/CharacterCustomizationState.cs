using MudBucket.Interfaces;
using MudBucket.Systems;

namespace MudBucket.States
{
    public class CharacterCustomizationState : IPlayerState
    {
        public void ProcessInput(PlayerSession session, string input)
        {
            // Assuming input could be a sequence of customization commands or a signal to finish customization
            // Here, a simplified example is provided, and you might need a more complex handling mechanism

            if (input.Equals("done", StringComparison.OrdinalIgnoreCase))
            {
                session.SetState(new InGameState());
                session.SendToClient("Customization complete. Welcome to the game!", session.Client);
            }
            else
            {
                // Here, you would parse and apply customization settings
                // For example, "set race elf", "set class warrior", etc.
                session.SendToClient("Customization in progress... Enter 'done' when finished.", session.Client);
            }
        }
    }

}
