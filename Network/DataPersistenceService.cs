using MudBucket.Interfaces.MudBucket.Interfaces;

public class DataPersistenceService : IDataPersistenceService
{
    public async Task SaveAllDataAsync()
    {
        Console.WriteLine("Data saved.");
    }
}
