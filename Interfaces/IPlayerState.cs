using MudBucket.Systems;

namespace MudBucket.Interfaces
{
    public interface IPlayerState
    {
        void ProcessInput(PlayerSession session, string input);
    }

}
