using MudBucket.Interfaces;
using MudBucket.Systems;

namespace MudBucket.States
{
    public class ReturningPlayerLoginState : IPlayerState
    {
        public void ProcessInput(PlayerSession session, string input)
        {
            session.ProcessInput(input);
        }
    }
}
