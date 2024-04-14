﻿using MudBucket.Interfaces;

namespace MudBucket.Services.General
{
    public class MessageFormatter : IMessageFormatter
    {
        private readonly AnsiColorManager _ansiColorManager;
        private readonly bool _ansiColorEnabled;

        public MessageFormatter(bool ansiColorEnabled)
        {
            _ansiColorEnabled = ansiColorEnabled;
            _ansiColorManager = new AnsiColorManager();
        }

        public string FormatMessage(string message)
        {
            if (_ansiColorEnabled)
            {
                message = _ansiColorManager.ApplyColorCodes(message);
            }
            return message + "\r\n";  // Always add a new line for clean formatting
        }
    }
}