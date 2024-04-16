using MudBucket.Interfaces;
using MudBucket.Systems;
using System.Net.Sockets;

namespace MudBucket.Commands
{
    public class SetBattlePromptCommand : CommandBase
    {
        private readonly string _battlePromptFormat;
        public SetBattlePromptCommand(string[] parameters)
        {
            if (parameters == null || parameters.Length == 0)
                throw new ArgumentException("No format provided for SetBattlePrompt command.");

            _battlePromptFormat = string.Join(" ", parameters);
        }
        public override SessionState[] ValidStates => new[] { SessionState.JustConnected, SessionState.Playing };
        protected override async Task<bool> ExecuteCommand(TcpClient client, INetworkService networkService, PlayerSession session)
        {
            session.Player.BattlePromptFormat = _battlePromptFormat;
            await networkService.SendAsync("Battle prompt format updated.");
            return true;
        }
    }
}