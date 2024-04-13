using MudBucket.Interfaces;
using System.Text.Json;

namespace MudBucket.Systems
{
    public class GenericJsonLoader : IJsonLoader
    {
        public T LoadData<T>(string filePath)
        {
            try
            {
                string jsonContent = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<T>(jsonContent);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to load data from {filePath}", ex);
            }
        }
    }
}
