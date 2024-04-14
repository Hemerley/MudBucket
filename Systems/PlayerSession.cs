using MudBucket.Interfaces;
using MudBucket.Services.Commands;
using MudBucket.States;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace MudBucket.Systems
{
    public class PlayerSession
    {
        private readonly TcpClient _client;
        private IPlayerState _currentState;
        private readonly CommandHandler _commandHandler;
        private readonly bool _ansiColorEnabled;

        private readonly Dictionary<string, string> _colorMap = new Dictionary<string, string>
        {
            {"black", "\u001b[30m"}, {"red", "\u001b[31m"}, {"green", "\u001b[32m"},
            {"yellow", "\u001b[33m"}, {"blue", "\u001b[34m"}, {"magenta", "\u001b[35m"},
            {"cyan", "\u001b[36m"}, {"white", "\u001b[37m"}, {"dark_red", "\u001b[31;1m"},
            {"dark_green", "\u001b[32;1m"}, {"brown", "\u001b[33;1m"}, {"dark_blue", "\u001b[34;1m"},
            {"purple", "\u001b[35;1m"}, {"light_blue", "\u001b[36;1m"}, {"grey", "\u001b[37;1m"}
        };

        public PlayerSession(TcpClient client, CommandHandler commandHandler, bool ansiColorEnabled)
        {
            _client = client;
            _commandHandler = commandHandler;
            _ansiColorEnabled = ansiColorEnabled;
            InitializeSession();
        }

        public TcpClient Client => _client;

        private void InitializeSession()
        {
            SetState(new NewConnectionState());
            SendWelcomeMessage();
        }

        public void SendToClient(string message)
        {
            try
            {
                var stream = _client.GetStream();
                var formattedMessage = FormatMessage(message);
                var buffer = Encoding.ASCII.GetBytes(formattedMessage);
                stream.Write(buffer, 0, buffer.Length);
                stream.Flush(); // Ensure the data is sent immediately
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending message to client: {ex.Message}");
            }
        }

        public void ProcessInput(string input)
        {
            // Placeholder: Handle incoming text input from client
            // Parse input, determine command or text, then process accordingly
            _commandHandler.HandleCommandAsync(input, _client);
        }

        private string FormatMessage(string message)
        {
            if (_ansiColorEnabled)
            {
                message = ApplyColorCodes(message) + "\u001b[0m"; // Reset color after message
            }
            return message + "\r\n"; // Standard new line for MUD clients
        }

        private string ApplyColorCodes(string message)
        {
            foreach (var color in _colorMap)
            {
                message = message.Replace($"[{color.Key}]", color.Value);
            }
            return message;
        }

        private void SetState(IPlayerState newState)
        {
            _currentState = newState;
        }

        private void SendWelcomeMessage()
        {
            string art = @"
[blue]                \||/
                |  @___oo
      /\  /\   / (__,,,,|
     ) /^\) ^\/ _)
     )   /^\/   _)
     )   _ /  / _)
 /\  )/\/ ||  | )_)
<  >      |(,,) )__)
 ||      /    \)___)\
 | \____(      )___) )___
  \______(_______;;; __;;;
[red]Welcome to MudBucket![white]
";

            SendToClient(art);
        }

    }
}