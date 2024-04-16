using MudBucket.Interfaces;
using MudBucket.Services.General;
using System.Text.Json;

namespace MudBucket.Services.Json
{
    public class GenericJsonWriter : IJsonWriter
    {
        public void WriteData<T>(T data, string filePath)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };
                string jsonContent = JsonSerializer.Serialize(data, options);
                string encryptedJsonContent = CryptoUtils.EncryptString(jsonContent);
                File.WriteAllText(filePath, encryptedJsonContent);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to write data to {filePath}: {ex.Message}", ex);
            }
        }
    }
}
