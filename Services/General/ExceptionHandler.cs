using MudBucket.Interfaces;

namespace MudBucket.Services.General
{
    public static class ExceptionHandler
    {
        public static void Handle(Exception ex, ILogger logger)
        {
            logger.Error($"An error occurred: {ex.Message}");
        }
    }
}