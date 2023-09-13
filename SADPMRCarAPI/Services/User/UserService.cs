using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SADPMRCarAPI.Data;
using SADPMRCarAPI.DTO.UserDto.UserDto;
using SADPMRCarAPI.DTO.UserDto;
using SADPMRCarAPI.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web.Mvc;

namespace SADPMRCarAPI.Services.User
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUserIdentity> _userManager;
        private readonly IConfiguration _configuration;
   

        public UserService(UserManager<ApplicationUserIdentity> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<IActionResult> Login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
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

            return new UnauthorizedResult();
        }

        public async Task<IActionResult> Register(UserDto model)
        {

            var userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
            {
                return new ConflictObjectResult(new ServiceResponse { Status = "Error", Message = "User already exists" });
            }

            var user = new ApplicationUserIdentity
            {
                UserName = model.Email,
                Password = model.Password,
                Email = model.Email,
                Name = model.Name,
                Surname = model.Surname,
                JobTitleId = model.JobTitleId,
                EmployeeNumber = model.EmployeeNumber,
                DepartmentId = model.DepartmentId
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return new OkObjectResult(new ServiceResponse { Status = "Success", Message = "User created successfully" });
            }

            var errors = result.Errors.Select(e => e.Description);
            var message = string.Join(", ", errors);
            return new BadRequestObjectResult(new ServiceResponse { Status = "Error", Message = message });
        }

        public IActionResult GetAllUsers()
        {
            var users = _userManager.Users.ToList();
            var userDtos = users.Select(user => new UserDto
            {
                Email = user.Email,
                Name = user.Name,
                Surname = user.Surname,
                JobTitleId = (int)user.JobTitleId,
                EmployeeNumber = user.EmployeeNumber,
                DepartmentId = user.DepartmentId
                // Map other properties as needed
            }).ToList();

            return new OkObjectResult(userDtos);
        }

        public async Task<IActionResult> GetUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return new NotFoundObjectResult(new ServiceResponse { Status = "Error", Message = "User not found" });
            }

            var userDto = new UserDto
            {
                Email = user.Email,
                Name = user.Name,
                Surname = user.Surname,
                JobTitleId = (int)user.JobTitleId,
                EmployeeNumber = user.EmployeeNumber,
                DepartmentId = user.DepartmentId

                // Map other properties as needed
            };

            return new OkObjectResult(userDto);
        }

        public async Task<IActionResult> UpdateUser(string id, UserDto model)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return new NotFoundObjectResult(new ServiceResponse { Status = "Error", Message = "User not found" });
            }

            user.Email = model.Email;
            user.UserName = model.Email;
            user.Name = model.Name;
            user.Surname = model.Surname;
            // Update other properties as needed

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return new OkObjectResult(new ServiceResponse { Status = "Success", Message = "User updated successfully" });
            }
            else
            {
                var errors = result.Errors.Select(e => e.Description);
                var message = string.Join(", ", errors);
                return new BadRequestObjectResult(new ServiceResponse { Status = "Error", Message = message });
            }
        }

        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return new NotFoundObjectResult(new ServiceResponse { Status = "Error", Message = "User not found" });
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return new OkObjectResult(new ServiceResponse { Status = "Success", Message = "User deleted successfully" });
            }
            else
            {
                var errors = result.Errors.Select(e => e.Description);
                var message = string.Join(", ", errors);
                return new BadRequestObjectResult(new ServiceResponse { Status = "Error", Message = message });
            }
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return new BadRequestObjectResult(new ServiceResponse { Status = "Error", Message = "Invalid confirmation link" });
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new BadRequestObjectResult(new ServiceResponse { Status = "Error", Message = "User not found" });
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                // Update the database
                user.EmailConfirmed = true;
                var updateResult = await _userManager.UpdateAsync(user);
                if (updateResult.Succeeded)
                {
                    return new OkObjectResult(new ServiceResponse { Status = "Success", Message = "Email confirmed successfully" });
                }
                else
                {
                    var errors = updateResult.Errors.Select(e => e.Description);
                    var message = string.Join(", ", errors);
                    return new BadRequestObjectResult(new ServiceResponse { Status = "Error", Message = message });
                }
            }
            else
            {
                var errors = result.Errors.Select(e => e.Description);
                var message = string.Join(", ", errors);
                return new BadRequestObjectResult(new ServiceResponse { Status = "Error", Message = message });
            }
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(10),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }
    }
}
