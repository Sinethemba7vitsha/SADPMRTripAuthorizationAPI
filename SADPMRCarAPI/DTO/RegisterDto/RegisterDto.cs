using SADPMRCarAPI.DTO.DepartmentDto;

namespace SADPMRCarAPI.DTO.RegisterDto
{
    public class RegisterDto
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public AddDepartmentDto? Department { get; set; }
        public int EmployeeNumber { get; set; }
    }
}
