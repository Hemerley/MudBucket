namespace MudBucket.Interfaces
{
    public interface IJsonLoader
    {
        T LoadData<T>(string filePath, bool isEncrypted);
    }
}
