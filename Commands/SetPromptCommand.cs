using MudBucket.Interfaces;
using MudBucket.Systems;
using System.Net.Sockets;

namespace MudBucket.Commands
{
    public class SetPromptCommand : CommandBase
    {
        private readonly string _promptFormat;
        public SetPromptCommand(string[] parameters)
        {
            if (parameters == null || parameters.Length == 0)
                throw new ArgumentException("No format provided for SetPrompt command.");

            _promptFormat = string.Join(" ", parameters);
        }
        public override SessionState[] ValidStates => new[] { SessionState.JustConnected, SessionState.Playing };
        protected override async Task<bool> ExecuteCommand(TcpClient client, INetworkService networkService, PlayerSession session)
        {
            session.Player.PromptFormat = _promptFormat;
            await networkService.SendAsync("Prompt format updated.");
            return true;
        }
    }
}