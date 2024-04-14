namespace MudBucket.Interfaces
{
    public interface IStateManager
    {
        void SetState(IPlayerState newState);
        void Cleanup();
    }
}
