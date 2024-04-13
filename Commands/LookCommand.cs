﻿using System.Net.Sockets;

namespace MudBucket.Commands
{
    public class LookCommand : CommandBase
    {
        public override bool Execute(TcpClient client)
        {
            SendToClient("You look around and see...", client);
            return true;
        }
    }
}