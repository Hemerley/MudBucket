using MudBucket.Interfaces;
using System.Net.Sockets;

namespace MudBucket.Systems
{
    public enum SessionState
    {
        JustConnected,
        Playing
    }

    public class PlayerSession
    {
        private readonly TcpClient _client;
        private readonly INetworkService _networkService;
        private readonly ICommandParser _commandParser;
        private readonly IMessageFormatter _messageFormatter;
        private SessionState _currentState;

        public PlayerSession(TcpClient client, INetworkService networkService, ICommandParser commandParser, IMessageFormatter messageFormatter)
        {
            _client = client;
            _networkService = networkService;
            _commandParser = commandParser;
            _messageFormatter = messageFormatter;
            _currentState = SessionState.JustConnected;
            InitializeSession();
        }

        public SessionState CurrentState => _currentState;

        public void ChangeStateToPlaying()
        {
            if (_currentState == SessionState.JustConnected)
            {
                _currentState = SessionState.Playing;
            }
        }

        private async void InitializeSession()
        {
            await SendWelcomeMessage().ConfigureAwait(false);
        }

        public async Task ProcessInput(string input)
        {
            var success = await _commandParser.ParseCommand(input, _client, this).ConfigureAwait(false);
            if (!success)
            {
                Console.WriteLine("Failed to process command.");
            }
        }

        private async Task SendWelcomeMessage()
        {
            string art = @"
[green]                \||/
                |  [yellow]@[green]___[red]oo[green]
      /\  /\   / (__[white],[yellow],[red],[yellow],[green]|
     ) /^\) ^\/ [yellow]_[white])[green]
     )   /^\/   [yellow]_[white])[green]
     )   _ /  / [yellow]_[white])[green]
 /\  )/\/ ||  | [white])[yellow]_[white])[green]
<  >      |[white]([yellow],,[white]) )[yellow]__[white])[green]
 ||      /    \[white])[yellow]___[white])[green]\
 | \____(      [white])[yellow]___[white]) )[yellow]___[green]
  \______(_______[yellow];;;[green] __[yellow];;;[green]
[server]Welcome to MudBucket[white]![server] Are you a [server_info][new] [server]or [server_info][returning][server] player[white]?";
            await SendMessageAsync(art).ConfigureAwait(false);
        }

        public async Task SendMessageAsync(string message)
        {
            var formattedMessage = _messageFormatter.FormatMessage(message);
            await _networkService.SendAsync(formattedMessage).ConfigureAwait(false);
        }

        public async Task HandleSession()
        {
            try
            {
                while (_client.Connected)
                {
                    string message = await _networkService.ReceiveAsync().ConfigureAwait(false);
                    if (string.IsNullOrEmpty(message))
                        break;

                    await ProcessInput(message.Trim()).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during session: {ex.Message}");
            }
            finally
            {
                Cleanup();
            }
        }

        public void Disconnect()
        {
            _client.Close();
        }

        public void Cleanup()
        {
            _networkService.Close();
            Disconnect();
        }
    }
}
