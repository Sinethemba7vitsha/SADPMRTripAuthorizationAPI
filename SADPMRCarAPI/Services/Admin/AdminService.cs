using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SADPMRCarAPI.Data;
using SADPMRCarAPI.DTO.UserDto.AdminDto;
using SADPMRCarAPI.DTO.UserDto.UserDto;
using SADPMRCarAPI.DTO.UserDto;
using SADPMRCarAPI.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using System.Text;
using static Microsoft.Graph.Constants;
using Microsoft.AspNetCore.Mvc;

namespace SADPMRCarAPI.Services.Admin
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<ApplicationUserIdentity> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUserIdentity> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public AdminService(
            UserManager<ApplicationUserIdentity> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUserIdentity> signInManager,
            IMapper mapper,
            IConfiguration configuration,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _context = context;
        }




        public async Task<IActionResult> RegisterAdmin([FromBody] CreateAdminDto adminDto)
        {
            var userExists = await _userManager.FindByEmailAsync(adminDto.Email);
            if (userExists != null)
            {
                return new ConflictObjectResult(new ServiceResponse { Status = "Error", Message = "User already exists" });
            }

            var user = new ApplicationUserIdentity
            {
                UserName = adminDto.Email,
                Password = adminDto.Password,
                Email = adminDto.Email,
                Name = adminDto.Name,
                Surname = adminDto.Surname,
                JobTitleId = adminDto.JobTitleId,
                EmployeeNumber = adminDto.EmployeeNumber,
                DepartmentId = adminDto.DepartmentId
            };

            var result = await _userManager.CreateAsync(user, adminDto.Password);
            if (result.Succeeded)
            {
                return new OkObjectResult(new ServiceResponse { Status = "Success", Message = "User created successfully" });
            }

            var errors = result.Errors.Select(e => e.Description);
            var message = string.Join(", ", errors);
            return new BadRequestObjectResult(new ServiceResponse { Status = "Error", Message = message });
        }

        public async Task<IActionResult> Login(LoginDto model)
        {
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = GetToken(authClaims);

                return new OkObjectResult(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }

            return new NotFoundObjectResult("Login Denied.");
        }

        public async Task<IActionResult> UpdateUser(string userId, UpdateUserModel model)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new NotFoundObjectResult(new ServiceResponse
                {
                    Status = "Error",
                    Message = "User not found!"
                });
            }

            user.Email = model.Email;
            user.UserName = model.Email;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return new NotFoundObjectResult(new ServiceResponse
                {
                    Status = "Error",
                    Message = "User update failed! Please check user details and try again."
                });
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, userRoles.ToArray());

            var rolesToAdd = new List<string> { UserRolesModel.User };
            if (model.IsAdmin)
            {
                rolesToAdd.Add(UserRolesModel.Admin);
            }

            foreach (var role in rolesToAdd)
            {
                if (await _roleManager.RoleExistsAsync(role))
                {
                    await _userManager.AddToRoleAsync(user, role);
                }
            }

            return new OkObjectResult(new ServiceResponse
            {
                Status = "Success",
                Message = "User updated successfully!"
            });
        }

        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new NotFoundObjectResult(new ServiceResponse
                {
                    Status = "Error",
                    Message = "User not found!"
                });
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return new NotFoundObjectResult(new ServiceResponse
                {
                    Status = "Error",
                    Message = "User deletion failed! Please try again."
                });
            }

            return new OkObjectResult(new ServiceResponse
            {
                Status = "Success",
                Message = "User deleted successfully!"
            });
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }

       
    }
}
