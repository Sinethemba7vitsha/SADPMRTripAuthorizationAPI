using Microsoft.AspNetCore.Mvc;
using SADPMRCarAPI.DTO.UserDto.UserDto;
using SADPMRCarAPI.DTO.UserDto;

namespace SADPMRCarAPI.Services.User
{
    public interface IUserService
    {
        Task<IActionResult> Login(LoginDto model);
        Task<IActionResult> Register(UserDto model);
        IActionResult GetAllUsers();
        Task<IActionResult> GetUser(string id);
        Task<IActionResult> UpdateUser(string id, UserDto model);
        Task<IActionResult> DeleteUser(string id);
        Task<IActionResult> ConfirmEmail(string userId, string token);
    }
}
