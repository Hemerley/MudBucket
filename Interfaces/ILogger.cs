namespace MudBucket.Interfaces
{
    public interface ILogger
    {
        void Information(string message);
        void Debug(string message);
        void Error(string message);
    }

}
