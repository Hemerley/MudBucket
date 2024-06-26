﻿using System.Net.Sockets;
using System.Text;
using MudBucket.Systems;
using Newtonsoft.Json;

namespace MudBucket.Structures
{
    public class Player
    {
        public string Name { get; set; }
        public string Password { get; set; }
        [JsonIgnore]
        public PlayerSession Session { get; private set; }
        public string Race { get; set; }
        public string PlayerClass { get; set; }
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
        public int CampaignPoints { get; set; }
        public int Tier { get; set; }
        public bool HeroStatus { get; set; }
        public int Remort { get; set; }
        public Room CurrentRoom { get; set; }
        public Dictionary<string, int> Attributes { get; set; }
        public List<Item> Inventory { get; set; }
        public Dictionary<EquipmentSlot, Item> Equipment { get; set; }
        public List<Quest> Quests { get; set; }
        public string PromptFormat { get; set; }
        public string BattlePromptFormat { get; set; }
        public enum EquipmentSlot
        {
            Head, Neck, Torso, Legs, Feet, Hands, Arms, Shield, Ring, Weapon
        }
        public Player(PlayerSession playerSession)
        {
            Session = playerSession;
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
            CampaignPoints = 0;
            Tier = 1;
            HeroStatus = false;
            Remort = 0;
            PromptFormat = "HP: {health}/{maxHealth} Mana: {mana}/{maxMana}";
            BattlePromptFormat = "HP: {health}/{maxHealth} Mana: {mana}/{maxMana} Enemy HP: {enemyHealth}";
        }
        private void InitializeEquipmentSlots()
        {
            foreach (EquipmentSlot slot in Enum.GetValues(typeof(EquipmentSlot)))
            {
                Equipment[slot] = null;
            }
        }
        public async Task SendMessage(string message)
        {
            await Session.SendMessageAsync(message);
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
