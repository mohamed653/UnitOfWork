namespace WebApiLab2.DTOs
{
    public class StudentDto
    {
        public int StId { get; set; }
        public string StName { get; set; }
        public string? StAddress { get; set; }
        public int StAge { get; set; }
        public string DeptName { get; set; }

        public string SupervisorName { get; set; }
    }
}
