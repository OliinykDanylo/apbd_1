using Microsoft.Data.SqlClient;
using Teachers.Repositories;

public class RecordRepository : IRecordRepository
{
        private readonly string _connectionString;

        public RecordRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<Record>> GetAllAsync()
        {
            var records = new List<Record>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(@"
                SELECT r.Id, r.ExecutionTime, r.CreatedAt,
                       t.Id AS TeacherId, t.FirstName, t.MiddleName, t.LastName, t.Email, t.Level,
                       at.Name AS AcademicTitle,
                       rt.Name AS RecordType,
                       task.Id AS TaskId, task.Name AS TaskName, task.Description, task.RequiredLevel, task.MinRequiredTime
                FROM Records r
                JOIN Teacher t ON t.Id = r.TeacherId
                JOIN AcademicTitle at ON at.Id = t.AcademicTitleId
                JOIN RecordType rt ON rt.Id = r.RecordTypeId
                JOIN Task1 task ON task.Id = r.TaskId", connection);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                records.Add(new Record
                {
                    Id = (int)reader["Id"],
                    ExecutionTime = (decimal)reader["ExecutionTime"],
                    CreatedAt = (DateTime)reader["CreatedAt"],
                    Teacher = new Teacher
                    {
                        Id = (int)reader["TeacherId"],
                        FirstName = reader["FirstName"].ToString(),
                        MiddleName = reader["MiddleName"] as string,
                        LastName = reader["LastName"].ToString(),
                        Email = reader["Email"].ToString(),
                        Level = (int)reader["Level"],
                        AcademicTitle = reader["AcademicTitle"].ToString()
                    },
                    RecordType = reader["RecordType"].ToString(),
                    Task = new TaskModel
                    {
                        Id = (int)reader["TaskId"],
                        Name = reader["TaskName"].ToString(),
                        Description = reader["Description"].ToString(),
                        RequiredLevel = reader["RequiredLevel"] as int?,
                        MinimalRequiredTime = reader["MinRequiredTime"] as decimal?
                    }
                });
            }

            return records;
        }

        public async Task<Teacher?> GetTeacherByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand("SELECT * FROM Teacher WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);
            using var reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync()) return null;

            return new Teacher
            {
                Id = (int)reader["Id"],
                FirstName = reader["FirstName"].ToString(),
                MiddleName = reader["MiddleName"] as string,
                LastName = reader["LastName"].ToString(),
                Email = reader["Email"].ToString(),
                AcademicTitleId = (int)reader["AcademicTitleId"],
                Level = (int)reader["Level"]
            };
        }

        public async Task<string?> GetRecordTypeNameByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand("SELECT Name FROM RecordType WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);
            return await command.ExecuteScalarAsync() as string;
        }

        public async Task<TaskModel?> GetTaskByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand("SELECT * FROM Task WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);
            using var reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync()) return null;

            return new TaskModel
            {
                Id = (int)reader["Id"],
                Name = reader["Name"].ToString(),
                Description = reader["Description"].ToString(),
                RequiredLevel = reader["RequiredLevel"] as int?,
                MinimalRequiredTime = reader["MinRequiredTime"] as decimal?
            };
        }

        public async Task<int> CreateTaskAsync(TaskModel task)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(@"
                INSERT INTO Task (Name, Description, RequiredLevel, MinRequiredTime)
                OUTPUT INSERTED.Id
                VALUES (@Name, @Description, @RequiredLevel, @MinRequiredTime)", connection);

            command.Parameters.AddWithValue("@Name", task.Name);
            command.Parameters.AddWithValue("@Description", task.Description);
            command.Parameters.AddWithValue("@RequiredLevel", (object?)task.RequiredLevel ?? DBNull.Value);
            command.Parameters.AddWithValue("@MinRequiredTime", (object?)task.MinimalRequiredTime ?? DBNull.Value);

            return (int)await command.ExecuteScalarAsync();
        }

        public async Task<int> CreateRecordAsync(Record record)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(@"
                INSERT INTO Records (TeacherId, RecordTypeId, TaskId, ExecutionTime, CreatedAt)
                OUTPUT INSERTED.Id
                VALUES (@TeacherId, @RecordTypeId, @TaskId, @ExecutionTime, @CreatedAt)", connection);

            command.Parameters.AddWithValue("@TeacherId", record.Teacher.Id);
            command.Parameters.AddWithValue("@RecordTypeId", record.RecordTypeId);
            command.Parameters.AddWithValue("@TaskId", record.Task.Id);
            command.Parameters.AddWithValue("@ExecutionTime", record.ExecutionTime);
            command.Parameters.AddWithValue("@CreatedAt", record.CreatedAt);

            return (int)await command.ExecuteScalarAsync();
        }
}