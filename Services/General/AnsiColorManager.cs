namespace MudBucket.Services.General
{
    public class AnsiColorManager
    {
        private readonly Dictionary<string, string> _colorMap = new Dictionary<string, string>
    {
        {"black", "\u001b[30m"}, {"red", "\u001b[31m"}, {"green", "\u001b[32m"},
        {"yellow", "\u001b[33m"}, {"blue", "\u001b[34m"}, {"magenta", "\u001b[35m"},
        {"cyan", "\u001b[36m"}, {"white", "\u001b[37m"}, {"dark_red", "\u001b[31;1m"},
        {"dark_green", "\u001b[32;1m"}, {"brown", "\u001b[33;1m"}, {"dark_blue", "\u001b[34;1m"},
        {"purple", "\u001b[35;1m"}, {"light_blue", "\u001b[36;1m"}, {"grey", "\u001b[37;1m"}
    };

        public string ApplyColorCodes(string message)
        {
            foreach (var color in _colorMap)
            {
                message = message.Replace($"[{color.Key}]", color.Value);
            }
            return message;
        }

        public string ResetColor() => "\u001b[0m";
    }
}
