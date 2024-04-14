using MudBucket.Interfaces;
using MudBucket.Services.Commands;
using MudBucket.States;
using System.Net.Sockets;
using System.Text;

namespace MudBucket.Systems
{
    public class PlayerSession
    {
        private readonly TcpClient _client;
        private IPlayerState _currentState;
        private readonly CommandHandler _commandHandler;

        public PlayerSession(TcpClient client, CommandHandler commandHandler)
        {
            _client = client;
            _commandHandler = commandHandler;
            SetState(new NewConnectionState());
            SendWelcomeMessage();
        }

        public TcpClient Client => _client;

        public void SetState(IPlayerState newState)
        {
            _currentState = newState;
        }

        public void SendWelcomeMessage()
        {
            string welcomeMessage = @"
                ___  ___          _______            _        _   
                |  \/  |         | | ___ \          | |      | |  
                | .  . |_   _  __| | |_/ /_   _  ___| | _____| |_ 
                | |\/| | | | |/ _` | ___ \ | | |/ __| |/ / _ \ __|
                | |  | | |_| | (_| | |_/ / |_| | (__|   <  __/ |_ 
                \_|  |_/\__,_|\__,_\____/ \__,_|\___|_|\_\___|\__|            
                Welcome to MudBucket!
                Are you a [new] player or a [returning] player?";
            SendToClient(welcomeMessage, _client);
        }


        public void ProcessInput(string input)
        {
            _currentState.ProcessInput(this, input);
        }

        public void SendToClient(string message, TcpClient client)
        {
            var stream = client.GetStream();
            var buffer = Encoding.ASCII.GetBytes(message + "\r\n");
            stream.Write(buffer, 0, buffer.Length);
        }
    }
}