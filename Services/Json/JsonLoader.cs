using MudBucket.Interfaces;
using MudBucket.Services.General;  // Ensure this is correctly referenced
using System.Text.Json;

namespace MudBucket.Services.Json
{
    public class GenericJsonLoader : IJsonLoader
    {
        public T LoadData<T>(string filePath, bool isEncrypted)
        {
            try
            {
                string jsonContent = File.ReadAllText(filePath);
                if (isEncrypted)
                {
                    jsonContent = CryptoUtils.DecryptString(jsonContent);
                }
                return JsonSerializer.Deserialize<T>(jsonContent);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to load data from {filePath}: {ex.Message}", ex);
            }
        }
    }
}
