using MudBucket.Commands;
using MudBucket.Interfaces;


namespace MudBucket.Systems
{
    public static class CommandFactory
    {
        public static ICommand CreateCommand(string commandType, string parameter = null)
        {
            switch (commandType.ToLower())
            {
                case "look":
                    return new LookCommand();
                case "move":
                    return new MoveCommand(parameter); // Assuming 'parameter' is the direction
                case "quit":
                    return new QuitCommand();
                default:
                    return new UnknownCommand();
            }
        }
    }
}
