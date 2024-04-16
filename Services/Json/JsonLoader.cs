using MudBucket.Interfaces;
using MudBucket.Services.General;  // Ensure this is correctly referenced
using System.Text.Json;

namespace MudBucket.Services.Json
{
    public class GenericJsonLoader : IJsonLoader
    {
        public T LoadData<T>(string filePath)
        {
            try
            {
                string encryptedJsonContent = File.ReadAllText(filePath);
                string decryptedJsonContent = CryptoUtils.DecryptString(encryptedJsonContent);
                return JsonSerializer.Deserialize<T>(decryptedJsonContent);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to load data from {filePath}: {ex.Message}", ex);
            }
        }
    }
}
