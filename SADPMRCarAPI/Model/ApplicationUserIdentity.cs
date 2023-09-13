using Microsoft.AspNetCore.Identity;

namespace SADPMRCarAPI.Model
{
    public class ApplicationUserIdentity :  IdentityUser
    {
        public string Password { get; set; }    
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public int? JobTitleId { get; set; }   
        public int EmployeeNumber { get; set; }
        public int DepartmentId { get; set; }
    }
}
