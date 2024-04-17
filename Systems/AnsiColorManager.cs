namespace MudBucket.Systems
{
    public class AnsiColorManager
    {

        private readonly Dictionary<string, string> _colorMap = new Dictionary<string, string>
        {
            // Regular colors
            {"black", "\u001b[30m"}, // Black
            {"red", "\u001b[31m"}, // Red
            {"green", "\u001b[32m"}, // Green
            {"yellow", "\u001b[33m"}, // Yellow
            {"blue", "\u001b[34m"}, // Blue
            {"magenta", "\u001b[35m"}, // Magenta
            {"cyan", "\u001b[36m"}, // Cyan
            {"white", "\u001b[37m"}, // White
            // Bright (bold) colors
            {"bright_black", "\u001b[30;1m"}, // Bright Black (often gray or dark gray)
            {"bright_red", "\u001b[31;1m"}, // Bright Red
            {"bright_green", "\u001b[32;1m"}, // Bright Green
            {"bright_yellow", "\u001b[33;1m"}, // Bright Yellow (often described as brown in some terminals)
            {"bright_blue", "\u001b[34;1m"}, // Bright Blue
            {"bright_magenta", "\u001b[35;1m"}, // Bright Magenta
            {"bright_cyan", "\u001b[36;1m"}, // Bright Cyan
            {"bright_white", "\u001b[37;1m"}, // Bright White
            // System Colors
            {"reset", "\u001b[0m" }, // Reset color
            {"bold", "\u001b[1m" }, // Bold
            {"underline", "\u001b[4m" }, // Underline
            {"blink", "\u001b[5m" }, // Blink
            {"reverse", "\u001b[7m" }, // Reverse
            {"invisible", "\u001b[8m" }, // Invisible
            // Server Colors
            {"server", "\u001b[36;1m" }, // Bright Cyan
            {"server_error", "\u001b[31;1m" }, // Bright Red
            {"server_warning", "\u001b[33;1m" }, // Bright Yellow
            {"server_info", "\u001b[32;1m" }, // Bright Green
            {"server_debug", "\u001b[35;1m" }, // Bright Magenta

        };
        public string ApplyColorCodes(string message)
        {
            foreach (var color in _colorMap)
            {
                message = message.Replace($"[{color.Key}]", color.Value);
            }
            return message + ResetColor();
        }
        public string ResetColor() => "\u001b[0m";
    }
}
