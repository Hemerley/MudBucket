namespace MudBucket.Structures
{
    public class Item
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsEquippable { get; set; }
        public bool IsConsumable { get; set; }
        public Dictionary<string, int> Effects { get; set; }

        public Item()
        {
            Effects = new Dictionary<string, int>();
        }

        public void Use(Player player)
        {
            if (IsConsumable)
            {
                foreach (var effect in Effects)
                {
                    if (player.Attributes.ContainsKey(effect.Key))
                    {
                        player.Attributes[effect.Key] += effect.Value;
                        player.SendMessage($"{Name} used: {effect.Key} increased by {effect.Value}.");
                    }
                }
            }
        }
    }
}
