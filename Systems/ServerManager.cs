using MudBucket.Interfaces;
using MudBucket.Network;

namespace MudBucket
{
    public class ServerManager
    {
        private readonly IScheduler _scheduler;
        private readonly TcpServer _server;

        public ServerManager(IScheduler scheduler, TcpServer server)
        {
            _scheduler = scheduler;
            _server = server;
        }

        public void StartServer()
        {
            _scheduler.Start();
            _server.Start();
        }

        public void StopServer()
        {
            _scheduler.Stop();
            _server.Stop();
        }
    }
}
