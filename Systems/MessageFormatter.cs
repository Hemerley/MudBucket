using MudBucket.Interfaces;
using MudBucket.Structures;
using System.Text;

namespace MudBucket.Systems
{
    public class MessageFormatter : IMessageFormatter
    {
        private readonly AnsiColorManager _ansiColorManager;
        private readonly bool _ansiColorEnabled;
        private PromptService _promptService;
        public MessageFormatter(bool ansiColorEnabled)
        {
            _promptService = new PromptService();
            _ansiColorEnabled = ansiColorEnabled;
            _ansiColorManager = new AnsiColorManager();
        }
        public string FormatMessage(string message, Player player)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (_ansiColorEnabled)
            {
                stringBuilder.AppendLine(_ansiColorManager.ApplyColorCodes(message));
            }

            stringBuilder.AppendLine();
            return stringBuilder.ToString();
        }
    }
}