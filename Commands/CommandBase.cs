using MudBucket.Interfaces;
using MudBucket.Systems;
using System.Net.Sockets;

public abstract class CommandBase : ICommand
{
    public abstract SessionState[] ValidStates { get; }

    public async Task<bool> Execute(TcpClient client, INetworkService networkService, PlayerSession session)
    {
        if (!ValidStates.Contains(session.CurrentState))
        {
            return false;
        }

        return await ExecuteCommand(client, networkService, session);
    }

    protected abstract Task<bool> ExecuteCommand(TcpClient client, INetworkService networkService, PlayerSession session);
}
