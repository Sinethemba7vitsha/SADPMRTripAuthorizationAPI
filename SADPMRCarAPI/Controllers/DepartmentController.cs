using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SADPMRCarAPI.DTO.DepartmentDto;
using SADPMRCarAPI.Services.DepartmentService;
using System.Data;

namespace SADPMRCarAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet("GetDepartments")]
        public async Task<IActionResult> GetDepartments()
        {
            var departments = await _departmentService.GetDepartments();
            return Ok(departments);
        }

        [HttpGet("{departmentId}")]
        public async Task<IActionResult> GetDepartmentById(int departmentId)
        {
            var department = await _departmentService.GetDepartmentById(departmentId);
            if (department == null)
            {
                return NotFound();
            }
            return Ok(department);
        }

        [HttpPost("AddDepartment")]
        public async Task<IActionResult> AddDepartment(AddDepartmentDto departmentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var newDepartment = await _departmentService.AddDepartment(departmentDto);
            return CreatedAtAction(nameof(GetDepartmentById), new { departmentId = newDepartment.DepartmentId }, newDepartment);
        }

        [HttpPut("UpdateDepartment/{departmentId}")]
        public async Task<IActionResult> UpdateDepartment(int departmentId, AddDepartmentDto departmentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updated = await _departmentService.UpdateDepartment(departmentId, departmentDto);
            if (!updated)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("DeleteDepartment/{departmentId}")]
        public async Task<IActionResult> DeleteDepartment(int departmentId)
        {
            var deleted = await _departmentService.DeleteDepartment(departmentId);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
