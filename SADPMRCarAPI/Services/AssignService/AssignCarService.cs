using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SADPMRCarAPI.Data;
using SADPMRCarAPI.Model;
using System.Security.Claims;

namespace SADPMRCarAPI.Services.AssignService
{
    public class AssignCarService : IAssignCarService
    {

        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUserIdentity> _userManager;

        public AssignCarService(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor , UserManager<ApplicationUserIdentity> userManager)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager; 
        }

        private string GetUserId() => _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        public async Task<IActionResult> GetMe()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            return new OkObjectResult(userId);
        }

        public async Task<IActionResult> AssignCarToTrip(int tripId, int carId)
        {
            var trip = await _dbContext.Trips.FindAsync(tripId);
            if (trip == null)
            {
                return new NotFoundObjectResult("Trip not found.");
            }

            var car = await _dbContext.Cars.FindAsync(carId);
            if (car == null)
            {
                return new NotFoundObjectResult("Car not found.");
            }

            
            var userIdentity = await _dbContext.Users.FirstOrDefaultAsync(c => c.Id == GetUserId());

            if (userIdentity.JobTitleId != 2)
            {
                return new UnauthorizedObjectResult("Only managers can assign cars to staff members.");
            }

            if (userIdentity.DepartmentId != car.DepartmentId)
            {
                return new BadRequestObjectResult("You can only assign cars to staff members in your department.");
            }

            trip.CarId = carId;
            trip.Car = car;
            await _dbContext.SaveChangesAsync();
            return new OkObjectResult("Car assigned to the trip successfully.");
        }
    }
}
