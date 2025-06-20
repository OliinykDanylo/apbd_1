namespace Teachers.Application;

public class RecordService : IRecordService
{
        private readonly RecordRepository _repository;

        public RecordService(RecordRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Record>> GetAllRecordsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<(bool Success, string? Error)> CreateRecordAsync(Record record)
        {
            var teacher = await _repository.GetTeacherByIdAsync(record.Teacher.Id);
            if (teacher == null)
                return (false, "Teacher not found.");

            var recordType = await _repository.GetRecordTypeNameByIdAsync(record.RecordTypeId);
            if (recordType == null)
                return (false, "Record type not found.");

            TaskModel? task = null;

            if (record.Task.Id > 0)
                task = await _repository.GetTaskByIdAsync(record.Task.Id);

            if (task == null)
            {
                if (string.IsNullOrEmpty(record.Task.Name) || string.IsNullOrEmpty(record.Task.Description))
                    return (false, "Task not found, and not enough info to create a new task.");

                int taskId = await _repository.CreateTaskAsync(record.Task);
                record.Task.Id = taskId;
            }
            else
            {
                record.Task = task;
            }

            if (task?.RequiredLevel is int requiredLevel && teacher.Level < requiredLevel)
                return (false, "Teacher level is below the required level for this task.");

            if (task?.MinimalRequiredTime is decimal minTime && record.ExecutionTime > minTime)
                return (false, "Execution time exceeds the minimal required time.");

            record.Teacher = teacher;

            await _repository.CreateRecordAsync(record);
            return (true, null);
        }
}