namespace MudBucket.Structures
{
    public enum ItemType
    {
        Weapon,
        Armor,
        Consumable,
        Quest,
        Key,
        Misc
    }
    public class Item
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsEquippable { get; set; }
        public bool IsConsumable { get; set; }
        public ItemType Type { get; set; }
        public int Weight { get; set; }
        public int Value { get; set; }
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
            else if (IsEquippable)
            {
                Equip(player);
            }
        }
        private void Equip(Player player)
        {
            player.SendMessage($"{Name} equipped.");
        }
    }
}