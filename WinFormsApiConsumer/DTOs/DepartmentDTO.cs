namespace WebApiLab2.DTOs
{
    public class DepartmentDTO
    {
        public int deptId { get; set; }
        public string? deptName { get; set; }
        public string? deptDesc { get; set; }

        public string? deptLocation { get; set; }

        public int numOfStudents { get; set; }
    }
}
