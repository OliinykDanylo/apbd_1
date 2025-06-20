public class Teacher
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int AcademicTitleId { get; set; }
        public string AcademicTitle { get; set; } = null!;
        public int Level { get; set; }
    }