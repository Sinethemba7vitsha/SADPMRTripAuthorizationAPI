
namespace SADPMRCarAPI.DTO.UserDto.UserDto
{
    public class UserDto
    {
        public string? Email { get; set; }
        public string? Password { get; set; }    
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public int JobTitleId { get; set; } 
        public int EmployeeNumber { get; set; }
        public int DepartmentId { get; set; }
    }
}
