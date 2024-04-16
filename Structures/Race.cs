namespace MudBucket.Structures
{
    public class Race
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Traits { get; set; }
        public Dictionary<string, int> BaseAttributes { get; set; }
        public Race()
        {
            Traits = new List<string>();
            BaseAttributes = new Dictionary<string, int>();
        }
    }
}
