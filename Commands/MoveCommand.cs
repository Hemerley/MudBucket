using Microsoft.Extensions.DependencyInjection;
using MudBucket.Interfaces;
using MudBucket.Structures;
using MudBucket.Systems;
using System.Net.Sockets;

namespace MudBucket.Commands
{
    public class MoveCommand : CommandBase
    {
        private readonly string _direction;
        public MoveCommand(string[] parameters)
        {
            if (parameters == null || parameters.Length == 0)
                throw new ArgumentException("No direction provided for Move command.");
            _direction = parameters[0];
        }
        public override SessionState[] ValidStates => new[] { SessionState.JustConnected, SessionState.Playing };
        protected override async Task<bool> ExecuteCommand(TcpClient client, INetworkService networkService, PlayerSession session)
        {
             await networkService.SendAsync($"[white][[server_info]INFO[white]][server]Attempting to move you[white]:[server_info] {_direction}", session.player);
            if (session.player.CurrentRoom.Exits.ContainsKey(_direction))
            {
                Room room = session.player.CurrentRoom;
                session.player.CurrentRoom = Program.ServiceProvider.GetRequiredService< GameDataRepository>().Rooms[session.player.CurrentRoom.Exits[_direction]];
                Program.ServiceProvider.GetRequiredService<GameDataRepository>().Rooms[room.Id].Players.Remove(session.player);
                Program.ServiceProvider.GetRequiredService<GameDataRepository>().Rooms[session.player.CurrentRoom.Id].Players.Add(session.player);
                Program.ServiceProvider.GetRequiredService<GameDataRepository>().Rooms[session.player.CurrentRoom.Id].DescribeRoom(session.player);
                session.player.SendMessage(Program.ServiceProvider.GetRequiredService<GameDataRepository>().Rooms[session.player.CurrentRoom.Id].GenerateAsciiMap(Program.ServiceProvider.GetRequiredService<GameDataRepository>().Rooms.ToDictionary(r => r.Id, r => r)));
            }
            else
            {
                await networkService.SendAsync("[white][[server_info]INFO[white]][server]You cannot move in that direction.", session.player);
            }
            return true;
        }
    }
}