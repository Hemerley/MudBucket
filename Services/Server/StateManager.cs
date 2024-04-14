using MudBucket.Interfaces;

namespace MudBucket.Services.Server
{
    public class StateManager : IStateManager
    {
        private IPlayerState? _currentState;

        public void SetState(IPlayerState newState)
        {
            _currentState = newState;
            // Additional logic for state transition can be added here.
            // Still A LOT of work needed here.
        }

        public void Cleanup()
        {
            _currentState = null;
        }
    }
}
