namespace Teachers.Application;

public interface IRecordService
{
    Task<List<Record>> GetAllRecordsAsync();
    Task<(bool Success, string? Error)> CreateRecordAsync(Record record);
    
}