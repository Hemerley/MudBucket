namespace MudBucket.Structures
{
    public class PlayerClass
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Abilities { get; set; }
        public PlayerClass()
        {
            Abilities = new List<string>();
        }
    }
}
