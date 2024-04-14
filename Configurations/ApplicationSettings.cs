namespace MudBucket.Configurations
{
    public class ApplicationSettings
    {
        public required string IPAddress { get; set; }
        public int Port { get; set; }
        public int BufferSize { get; set; }
        public int ConnectionTimeout { get; set; }
    }
}
