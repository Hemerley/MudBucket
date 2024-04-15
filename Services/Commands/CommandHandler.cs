using MudBucket.Interfaces;
using MudBucket.Services.General;
using MudBucket.Services.Server;
using System.Net.Sockets;

namespace MudBucket.Services.Commands
{
    public class CommandHandler : ICommandParser
    {
        private readonly ILogger _logger;

        public CommandHandler(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<bool> ParseCommand(string command, TcpClient client)
        {
            try
            {
                var args = command.Trim().Split(' ');
                var commandType = args[0].ToLower();
                var parameter = args.Length > 1 ? args[1] : null;

                ICommand cmd = CommandFactory.CreateCommand(commandType, parameter);
                return await cmd.Execute(client, new NetworkService(client, new MessageFormatter(true)));
            }
            catch (Exception ex)
            {
                _logger.Error("Error handling command: " + ex.Message);
                return false;
            }
        }
    }
}
