using SADPMRCarAPI.DTO.UserDto.AdminDto;
using SADPMRCarAPI.DTO.UserDto.UserDto;
using SADPMRCarAPI.DTO.UserDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;


namespace SADPMRCarAPI.Services.Admin
{
    public interface IAdminService
    {
        Task<IActionResult> RegisterAdmin([FromBody] CreateAdminDto adminDto);
        Task<IActionResult> Login([FromBody] LoginDto model);
        Task<IActionResult> UpdateUser(string userId, UpdateUserModel model);
        Task<IActionResult> DeleteUser(string userId);
    }

}
