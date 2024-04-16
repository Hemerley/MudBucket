using MudBucket.Commands;
using MudBucket.Interfaces;
using MudBucket.Systems;
using System.Net.Sockets;

namespace MudBucket.Services.Commands
{
    public static class CommandFactory
    {
        private static Dictionary<TcpClient, PlayerSession> _sessionMap;
        public static void Initialize(Dictionary<TcpClient, PlayerSession> sessionMap)
        {
            _sessionMap = sessionMap;
        }
        public static ICommand CreateCommand(string commandType, string[] parameters = null)
        {
            switch (commandType.ToLower())
            {
                case "look":
                    return new LookCommand();
                case "move":
                    if (parameters == null || parameters.Length == 0)
                        throw new ArgumentException("Move command requires a direction parameter.");
                    return new MoveCommand(parameters);
                case "quit":
                    if (_sessionMap == null)
                        throw new InvalidOperationException("Session map not initialized.");
                    return new QuitCommand(_sessionMap);
                case "help":
                    return new HelpCommand();
                case "new":
                    return new NewCommand();
                case "returning":
                    return new ReturningCommand();
                case "setprompt":
                    if (parameters == null || parameters.Length == 0)
                        throw new ArgumentException("SetPrompt command requires a format parameter.");
                    return new SetPromptCommand(parameters);
                case "setbattleprompt":
                    if (parameters == null || parameters.Length == 0)
                        throw new ArgumentException("SetBattlePrompt command requires a format parameter.");
                    return new SetBattlePromptCommand(parameters);
                default:
                    return new UnknownCommand();
            }
        }
    }
}