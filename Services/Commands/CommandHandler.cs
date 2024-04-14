using MudBucket.Interfaces;
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

        public async Task<bool> HandleCommandAsync(string command, TcpClient client)
        {
            try
            {
                // Parsing and executing the command are done here
                var args = command.Trim().Split(' ');
                var commandType = args[0].ToLower();
                var parameter = args.Length > 1 ? args[1] : null;

                ICommand cmd = CommandFactory.CreateCommand(commandType, parameter);
                return await Task.Run(() => cmd.Execute(client));
            }
            catch (Exception ex)
            {
                _logger.Error("Error handling command: " + ex.Message);
                return false;
            }
        }

        // If there is a need to explicitly separate parsing logic for testing or clarity, you can still keep it as a separate method:
        public Task<bool> ParseCommand(string command, TcpClient client)
        {
            return HandleCommandAsync(command, client);
        }
    }
}
