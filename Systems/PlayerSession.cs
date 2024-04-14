using MudBucket.Interfaces;
using MudBucket.States;
using System.Net.Sockets;

namespace MudBucket.Systems
{
    public class PlayerSession
    {
        private readonly TcpClient _client;
        private readonly INetworkService _networkService;
        private readonly ICommandParser _commandParser;
        private readonly IMessageFormatter _messageFormatter;
        private readonly IStateManager _stateManager;

        public PlayerSession(TcpClient client, INetworkService networkService, ICommandParser commandParser, IMessageFormatter messageFormatter, IStateManager stateManager)
        {
            _client = client;
            _networkService = networkService;
            _commandParser = commandParser;
            _messageFormatter = messageFormatter;
            _stateManager = stateManager;
            InitializeSession();
        }

        private async void InitializeSession()
        {
            _stateManager.SetState(new NewConnectionState());
            await SendWelcomeMessage().ConfigureAwait(false);
        }

        public async Task ProcessInput(string input)
        {
            var success = await _commandParser.ParseCommand(input, _client).ConfigureAwait(false);
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
[white]Welcome to MudBucket! Are you a [cyan][new] [white]or [cyan][returning][white] player?";
            var message = _messageFormatter.FormatMessage(art);
            await _networkService.SendAsync(message).ConfigureAwait(false);
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

        private void Cleanup()
        {
            _networkService.Close();
            _stateManager.Cleanup();
            Disconnect();
        }
    }
}
