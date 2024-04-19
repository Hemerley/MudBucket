using MudBucket.Structures;

namespace MudBucket.Interfaces
{
    public interface IMessageFormatter
    {
        string FormatMessage(string message, Player player);
    }
}
