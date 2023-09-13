using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SADPMRCarAPI.Data;
using SADPMRCarAPI.DTO.DepartmentDto;
using SADPMRCarAPI.Model;

namespace SADPMRCarAPI.Services.DepartmentService
{
    public class DepartmentService : IDepartmentService
    {
        private readonly ApplicationDbContext _context;

        public DepartmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Department>> GetDepartments()
        {
            return await _context.Departments.ToListAsync();
        }

        public async Task<Department> GetDepartmentById(int departmentId)
        {
            return await _context.Departments.FindAsync(departmentId);
        }

        public async Task<Department> AddDepartment(AddDepartmentDto departmentDto)
        {
            var newDepartment = new Department
            {
                DepartmentName = departmentDto.DepartmentName,
                
            };

            _context.Departments.Add(newDepartment);
            await _context.SaveChangesAsync();

            return newDepartment;
        }

        public async Task<bool> UpdateDepartment(int departmentId, AddDepartmentDto departmentDto)
        {
            var department = await _context.Departments.FindAsync(departmentId);
            if (department == null)
            {
                return false;
            }

            department.DepartmentName = departmentDto.DepartmentName;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteDepartment(int departmentId)
        {
            var department = await _context.Departments.FindAsync(departmentId);
            if (department == null)
            {
                return false;
            }

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();

            return true;
        }

         async Task<IEnumerable<Department>> IDepartmentService.GetDepartments()
        {
            return await _context.Departments.ToListAsync();
        }
    }
}
