using MudBucket.Commands;
using MudBucket.Interfaces;
using MudBucket.Systems;
using System.Collections.Generic;
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

        public static ICommand CreateCommand(string commandType, string? parameter = null)
        {
            switch (commandType.ToLower())
            {
                case "look":
                    return new LookCommand();
                case "move":
                    return new MoveCommand(parameter);
                case "quit":
                    if (_sessionMap == null)
                        throw new InvalidOperationException("Session map not initialized.");
                    return new QuitCommand(_sessionMap);
                default:
                    return new UnknownCommand();
            }
        }
    }
}
