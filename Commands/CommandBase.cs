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
                await networkService.SendAsync($"[white][[server_error]ERROR[white]][server]Error executing command[white]:[server] {ex.Message}");
                return false;
            }
        }

        protected abstract Task<bool> ExecuteCommand(TcpClient client, INetworkService networkService);
    }
}
