﻿using System.Collections.Generic;

namespace MudBucket.Structures
{
    public class Mob
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public Room CurrentRoom { get; set; }
        public Dictionary<string, int> Stats { get; set; }

        public Mob()
        {
            Stats = new Dictionary<string, int>
            {
                {"Attack", 5},
                {"Defense", 5}
            };
        }

        public void Attack(Player player)
        {
            player.Health -= Stats["Attack"];
            player.SendMessage($"{Name} attacks you for {Stats["Attack"]} damage.");
            if (player.Health <= 0)
            {
                player.SendMessage("You have died!");
            }
        }

        public void Move(Room newRoom)
        {
            CurrentRoom = newRoom;
            // Additional logic for moving the mob to a new room
        }

        public void Die()
        {
            // Additional logic for when the mob dies
        }
    }
}
