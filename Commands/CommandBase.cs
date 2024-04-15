using MudBucket.Interfaces;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace MudBucket.Commands
{
    public abstract class CommandBase : ICommand
    {
        public async Task<bool> Execute(TcpClient client, INetworkService networkService)
        {
            try
            {
                return await ExecuteCommand(client, networkService);
            }
            catch (Exception ex)
            {
                await networkService.SendAsync($"[bright_red]Error executing command: {ex.Message}");
                return false;
            }
        }

        // Each derived command class must implement this method.
        protected abstract Task<bool> ExecuteCommand(TcpClient client, INetworkService networkService);
    }
}
