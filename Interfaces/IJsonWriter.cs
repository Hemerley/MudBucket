namespace MudBucket.Interfaces
{
    public interface IJsonWriter
    {
        void WriteData<T>(T data, string filePath, bool encrypt);
    }
}
