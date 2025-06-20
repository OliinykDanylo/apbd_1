using Teachers.Models;

namespace Teachers.Repositories;

public interface IRecordRepository
{
    Task<List<Record>> GetAllAsync();
    Task<Teacher?> GetTeacherByIdAsync(int id);
    Task<int> CreateRecordAsync(Record record);
}