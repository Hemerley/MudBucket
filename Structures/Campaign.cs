namespace MudBucket.Structures
{
    public class Campaign
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Quest> Quests { get; set; }
        public Dictionary<string, int> Rewards { get; set; }
        public bool IsActive { get; set; }
        public Campaign()
        {
            Quests = new List<Quest>();
            Rewards = new Dictionary<string, int>();
            IsActive = false;
        }
        public void Start()
        {
            IsActive = true;
        }
        public void End()
        {
            IsActive = false;
        }
        public void CheckQuestCompletion(Player player)
        {
            bool allQuestsCompleted = true;
            foreach (var quest in Quests)
            {
                if (!player.Quests.Contains(quest))
                {
                    allQuestsCompleted = false;
                    break;
                }
            }
            if (allQuestsCompleted)
            {
                GrantRewards(player);
                End();
            }
        }
        private void GrantRewards(Player player)
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
}
