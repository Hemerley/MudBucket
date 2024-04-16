﻿using MudBucket.Interfaces;
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
            await networkService.SendAsync($"[white][[server_info]INFO[white]][server]Attempting to move you[white]:[server_info] {_direction}");
            return true;
        }
    }
}