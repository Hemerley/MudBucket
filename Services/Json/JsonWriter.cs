using MudBucket.Interfaces;
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
                File.WriteAllText(filePath, jsonContent);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to write data to {filePath}", ex);
            }
        }
    }
}
