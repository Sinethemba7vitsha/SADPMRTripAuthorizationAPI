using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph.Models;
using SADPMRCarAPI.Data;
using SADPMRCarAPI.DTO.CarServiceDto;
using SADPMRCarAPI.Model;
using System.Security.Claims;

namespace SADPMRCarAPI.Services.AssignCarToServiceService
{
    public class AssignCarToService : IAssignCarToService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
     
        public AssignCarToService(ApplicationDbContext context , IHttpContextAccessor hhtpContextAccessor, ApplicationDbContext dbContext)
        {
            _context = context;
            _httpContextAccessor = hhtpContextAccessor;
        }

  /*      private string GetUserId() => _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);*/

        public async Task<IActionResult> PutCarInService(int carId, AddCarServiceDto addCarServiceDto)
        {
            var car = await _context.Cars.Include(c => c.CarService).FirstOrDefaultAsync(c => c.CarId == carId);
            if (car == null)
            {
                return new NotFoundObjectResult("Car not found.");
            }

/*
            var userIdentity = await _dbContext.Users.FirstOrDefaultAsync(c => c.Id == GetUserId());


            if (userIdentity.DepartmentId != car.DepartmentId)
            {
                return new NotFoundObjectResult("You can only put cars in your department into service.");
            }
*/

            if (car.CarService != null)
            {
                return new BadRequestObjectResult("Car is already in service.");
            }

            var carService = new CarServiceModel
            {
                DateOfTheService = addCarServiceDto.DateOfTheService,
                CarAvailability = false
            };

            car.CarService = carService;
            await _context.SaveChangesAsync();

            return new OkObjectResult("Car service put into service successfully.Car is no longer available.");
        }



        public async Task<IActionResult> EndCarService(int carId)
        {
            // Find the car by its ID and include the CarService navigation property
            var car = await _context.Cars.Include(c => c.CarService).FirstOrDefaultAsync(c => c.CarId == carId);

            if (car == null)
            {
                return new NotFoundObjectResult("Car not found.");
            }

          /*  var userIdentity = await _dbContext.Users.FirstOrDefaultAsync(c => c.Id == GetUserId());


            if (userIdentity.DepartmentId != car.DepartmentId)
            {
                return new NotFoundObjectResult("You can only put cars out of service in your department.");
            }*/

            // Check if the car is not in service
            if (car.CarService == null)
            {
                return new NotFoundObjectResult("Car is not in service.");
            }

            // Remove the association with the car service
            var carService = car.CarService;
            car.CarService = null;

            // Update the CarServiceModel
            carService.CarAvailability = true;

            // Save the changes to the database
            await _context.SaveChangesAsync();

            return new OkObjectResult("Car service ended successfully. Car is now available.");
        }


    }
}
