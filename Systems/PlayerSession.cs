using MudBucket.Commands;
using MudBucket.Interfaces;
using MudBucket.Structures;
using System.Net.Sockets;
using System.Text;

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
        private readonly CommandHandler _commandParser;
        private readonly IMessageFormatter _messageFormatter;
        private SessionState _currentState;
        private PromptService _promptService;
        public Player player { get; set; }
        public string[] LastCommandArguments { get; set; }

        public PlayerSession(TcpClient client, INetworkService networkService, CommandHandler commandParser, IMessageFormatter messageFormatter)
        {
            _promptService = new PromptService();
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
            string[] parts = input.Split(' ', 2);
            string command = parts[0].Trim();
            string arguments = parts.Length > 1 ? parts[1] : "";
            LastCommandArguments = arguments.Split(' ');
            await _commandParser.ParseCommand(input, _client, this).ConfigureAwait(false);
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
            if (_currentState != SessionState.Playing)
            {
                var formattedMessage = _messageFormatter.FormatMessage(message, player);
                await _networkService.SendAsync(formattedMessage, player).ConfigureAwait(false);
            }else
            {
                var playerPrompt = _messageFormatter.FormatMessage(_promptService.GeneratePrompt(player), player);
                await _networkService.SendAsync(playerPrompt, player).ConfigureAwait(false);
                var formattedMessage = _messageFormatter.FormatMessage(message, player);
                await _networkService.SendAsync(formattedMessage, player).ConfigureAwait(false);
            }   
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