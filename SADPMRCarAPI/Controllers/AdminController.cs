using Microsoft.AspNetCore.Mvc;
using SADPMRCarAPI.DTO.UserDto;
using SADPMRCarAPI.DTO.UserDto.UserDto;
using SADPMRCarAPI.DTO.UserDto.AdminDto;
using SADPMRCarAPI.Services.Admin;

namespace SADPMRCarAPI.Controllers
{
 
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost("LoginUser")]
        public Task<IActionResult> Login([FromBody] LoginDto model)
        {
            return _adminService.Login(model);
        }

        [HttpPost("CreateAdmin")]
        public Task<IActionResult> RegisterAdmin([FromBody] CreateAdminDto adminDto)
        {
            return _adminService.RegisterAdmin(adminDto);
        }

       

        [HttpPut("update-user/{userId}")]
        public Task<IActionResult> UpdateUser(string userId, UpdateUserModel model)
        {
            return _adminService.UpdateUser(userId, model);
        }

        [HttpDelete("delete-user/{userId}")]
        public Task<IActionResult> DeleteUser(string userId)
        {
            return _adminService.DeleteUser(userId);
        }
    }
}

 