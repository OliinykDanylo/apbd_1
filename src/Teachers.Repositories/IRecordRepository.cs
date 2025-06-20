using Teachers.Models;

namespace Teachers.Repositories;

public interface IRecordRepository
{
    Task<List<Record>> GetAllAsync();
    Task<Teacher?> GetTeacherByIdAsync(int id);
    Task<int> CreateRecordAsync(Record record);
    Task<TaskModel?> GetTaskByIdAsync(int id);
    Task<int> CreateTaskAsync(TaskModel task);
    Task<string?> GetRecordTypeNameByIdAsync(int id);
}