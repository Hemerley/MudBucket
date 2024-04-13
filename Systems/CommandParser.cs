using System.Net.Sockets;
using MudBucket.Interfaces;
using MudBucket.Commands;

namespace MudBucket.Systems
{
    public class CommandParser : ICommandParser
    {
        public bool ParseCommand(string command, TcpClient client)
        {
            var args = command.Trim().Split(' ');
            var baseCommand = args[0].ToLower();

            ICommand cmd = baseCommand switch
            {
                "move" => new MoveCommand(args.Length > 1 ? args[1] : null),
                "look" => new LookCommand(),
                "quit" => new QuitCommand(),
                _ => new UnknownCommand()
            };

            return cmd.Execute(client);
        }
    }

    public class UnknownCommand : ICommand
    {
        public bool Execute(TcpClient client)
        {
            SendToClient("Unknown command.", client);
            return true;
        }

        private void SendToClient(string message, TcpClient client)
        {
            var stream = client.GetStream();
            var buffer = System.Text.Encoding.ASCII.GetBytes(message + "\r\n");
            stream.Write(buffer, 0, buffer.Length);
        }
    }
}
