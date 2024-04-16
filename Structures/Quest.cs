namespace MudBucket.Structures
{
    public class Quest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<QuestObjective> Objectives { get; set; }
        public Dictionary<string, int> Rewards { get; set; }
        public bool IsComplete { get; set; }
        public Quest()
        {
            Objectives = new List<QuestObjective>();
            Rewards = new Dictionary<string, int>();
        }
        public void CheckCompletion(Player player)
        {
            IsComplete = Objectives.All(o => o.IsCompleted);
            if (IsComplete)
            {
                AwardRewards(player);
                player.SendMessage($"Quest completed: {Name}. Rewards have been given.");
            }
        }
        private void AwardRewards(Player player)
        {
            foreach (var reward in Rewards)
            {
                switch (reward.Key)
                {
                    case "Experience":
                        player.GainExperience(reward.Value);
                        break;
                    case "Gold":
                        player.AddGold(reward.Value);
                        break;
                }
            }
        }
    }
    public class QuestObjective
    {
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public void CompleteObjective(Player player)
        {
            IsCompleted = true;
            player.SendMessage($"Objective completed: {Description}");
        }
    }
}