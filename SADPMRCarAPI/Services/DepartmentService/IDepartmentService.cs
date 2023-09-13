using SADPMRCarAPI.DTO.DepartmentDto;
using SADPMRCarAPI.Model;

namespace SADPMRCarAPI.Services.DepartmentService
{
    public interface IDepartmentService
    {
        Task<IEnumerable<Department>> GetDepartments();
        Task<Department> GetDepartmentById(int departmentId);
        Task<Department> AddDepartment(AddDepartmentDto departmentDto);
        Task<bool> UpdateDepartment(int departmentId, AddDepartmentDto departmentDto);
        Task<bool> DeleteDepartment(int departmentId);
    }


}
