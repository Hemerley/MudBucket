using MudBucket.Interfaces;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace MudBucket.Services.Commands
{
    public class CommandParser : ICommandParser
    {
        public async Task<bool> ParseCommand(string command, TcpClient client)
        {
            var args = command.Trim().Split(' ');
            var commandType = args[0].ToLower();
            var parameter = args.Length > 1 ? args[1] : null;

            ICommand cmd = CommandFactory.CreateCommand(commandType, parameter);
            return await Task.Run(() => cmd.Execute(client));
        }
    }
}
