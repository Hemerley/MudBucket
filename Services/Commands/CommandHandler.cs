using MudBucket.Interfaces;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace MudBucket.Services.Commands
{
    public class CommandHandler
    {
        private readonly ICommandParser _commandParser;
        private readonly ILogger _logger;

        public CommandHandler(ICommandParser commandParser, ILogger logger)
        {
            _commandParser = commandParser;
            _logger = logger;
        }

        public async Task<bool> HandleCommandAsync(string command, TcpClient client)
        {
            try
            {
                return await _commandParser.ParseCommand(command, client);
            }
            catch (Exception ex)
            {
                _logger.Error("Error handling command: " + ex.Message);
                return false;
            }
        }
    }
}
