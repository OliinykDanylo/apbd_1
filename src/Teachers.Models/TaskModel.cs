public class TaskModel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int? RequiredLevel { get; set; }
    public decimal? MinimalRequiredTime { get; set; }
}