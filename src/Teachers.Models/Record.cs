public class Record
{
    public int Id { get; set; }
    public Teacher Teacher { get; set; } = null!;
    public int RecordTypeId { get; set; }
    public string RecordType { get; set; } = null!;
    public TaskModel Task { get; set; } = null!;
    public decimal ExecutionTime { get; set; }
    public DateTime CreatedAt { get; set; }
}