namespace WebApiLab2.DTOs
{
    public class DepartmentDTO
    {
        public int DeptId { get; set; }
        public string? DeptName { get; set; }
        public string? DepartmentDescription { get; set;}

        public string? DepartmentLocation { get; set; }

        public int NumberOfStudents{ get; set; }
    }
}
