﻿using System.Net.Sockets;
using System.Text;

namespace MudBucket.Structures
{
    public class Player
    {
        public string Name { get; set; }
        public TcpClient Client { get; private set; } // Ensure client is only set in constructor
        public string Race { get; set; }
        public string Class { get; set; }
        public int Level { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public int Mana { get; set; }
        public int MaxMana { get; set; }
        public int Moves { get; set; }
        public int MaxMoves { get; set; }
        public int Gold { get; set; }
        public int TriviaPoints { get; set; }
        public int QuestPoints { get; set; }
        public int Experience { get; set; }
        public Room CurrentRoom { get; set; }
        public Dictionary<string, int> Attributes { get; set; }
        public List<Item> Inventory { get; set; }
        public Dictionary<EquipmentSlot, Item> Equipment { get; set; }
        public List<Quest> Quests { get; set; }

        public enum EquipmentSlot
        {
            Head, Neck, Torso, Legs, Feet, Hands, Arms, Shield, Ring, Weapon
        }

        public Player(TcpClient client)
        {
            Client = client;
            Attributes = new Dictionary<string, int>
            {
                {"Strength", 10},
                {"Intelligence", 10},
                {"Dexterity", 10},
                {"Wisdom", 10}
            };
            Inventory = new List<Item>();
            Equipment = new Dictionary<EquipmentSlot, Item>();
            Quests = new List<Quest>();
            InitializeEquipmentSlots();
        }

        private void InitializeEquipmentSlots()
        {
            foreach (EquipmentSlot slot in Enum.GetValues(typeof(EquipmentSlot)))
            {
                Equipment[slot] = null;
            }
        }

        public void EquipItem(Item item, EquipmentSlot slot)
        {
            if (item.IsEquippable)
            {
                if (Equipment[slot] != null)
                {
                    SendMessage($"Replacing {Equipment[slot].Name} with {item.Name}.");
                }

                Equipment[slot] = item;
                SendMessage($"{item.Name} equipped in the {slot} slot.");
            }
            else
            {
                SendMessage("This item cannot be equipped.");
            }
        }

        public void UseItem(Item item)
        {
            if (item.IsConsumable)
            {
                item.Use(this);
                Inventory.Remove(item);
                SendMessage($"{item.Name} used.");
            }
            else
            {
                SendMessage("This item cannot be used.");
            }
        }

        public void SendMessage(string message) // Now public
        {
            byte[] buffer = Encoding.ASCII.GetBytes(message + "\r\n");
            Client.GetStream().Write(buffer, 0, buffer.Length);
        }

        public void GainExperience(int xp)
        {
            Experience += xp;
            if (Experience >= Level * 1000)
            {
                Level++;
                Experience = 0;
                IncreaseStats();
                SendMessage($"Congratulations! You've reached level {Level}.");
            }
        }

        private void IncreaseStats()
        {
            MaxHealth += 10;
            Health = MaxHealth;
            MaxMana += 10;
            Mana = MaxMana;
            MaxMoves += 10;
            Moves = MaxMoves;
        }

        public void AddGold(int amount)
        {
            Gold += amount;
            SendMessage($"Added {amount} gold. Total gold now: {Gold}.");
        }

        public void RemoveQuest(Quest quest)
        {
            if (Quests.Contains(quest))
            {
                Quests.Remove(quest);
                SendMessage($"Quest removed: {quest.Name}");
            }
        }

        public void CheckQuestCompletion()
        {
            Quests.ForEach(quest => quest.CheckCompletion(this));
        }
    }
}
