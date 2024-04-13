namespace MudBucket.Systems
{
    using Serilog;

    public class SerilogLogger : Interfaces.ILogger
    {
        public void Information(string message) => Log.Information(message);
        public void Debug(string message) => Log.Debug(message);
        public void Error(string message) => Log.Error(message);
    }

}
