using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SADPMRCarAPI.Model;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Net;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SADPMRCarAPI.DTO.UserDto.UserDto;
using SADPMRCarAPI.DTO.UserDto;
using SADPMRCarAPI.DTO.UserDto.AdminDto;
using Microsoft.EntityFrameworkCore;
using SADPMRCarAPI.Data;
using Microsoft.AspNetCore.Http;
using SADPMRCarAPI.Services.User;

namespace SADPMRCarAPI.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("LoginUser")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            return await _userService.Login(model);
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> Register([FromBody] UserDto model)
        {
            return await _userService.Register(model);
        }

        [HttpGet("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            return _userService.GetAllUsers();
        }

        [HttpGet("GetUser/{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            return await _userService.GetUser(id);
        }

        [HttpPut("UpdateUser/{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UserDto model)
        {
            return await _userService.UpdateUser(id, model);
        }

        [HttpDelete("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            return await _userService.DeleteUser(id);
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            return await _userService.ConfirmEmail(userId, token);
        }

    }
}
