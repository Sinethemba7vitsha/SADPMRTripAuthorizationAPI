using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SADPMRCarAPI.Data;
using SADPMRCarAPI.DTO.CarAccessoriesDto;
using SADPMRCarAPI.Model;
using SADPMRCarAPI.Services.AssignAccessoriesToCar;
using SADPMRCarAPI.Services.UploadFile;
using System.Net.Http;
using System.Security.Claims;

namespace SADPMRCarAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AssignAccessoriesToCarController : ControllerBase
    {
        private readonly IUploadfileService _uploadfileService;
        private readonly IAssignAccessoriesToCarService _assignAccessoriesToCarService;
        private readonly ApplicationDbContext _dbContext;

        public AssignAccessoriesToCarController(IAssignAccessoriesToCarService assignAccessoriesToCarService, ApplicationDbContext dbContext , IUploadfileService uploadfileService)
        {
            _assignAccessoriesToCarService = assignAccessoriesToCarService;
            _dbContext = dbContext;
            _uploadfileService = uploadfileService; 
        }

        [HttpPost("cars/{carId}/accessories")]
        public async Task<IActionResult> AssignAccessoriesToCar(int carId, [FromForm] AddCarAccessoriesDto accessoriesDto)
        {
            return await _assignAccessoriesToCarService.AssignAccessoriesToCar(carId, accessoriesDto);
        }

        [HttpPost("cars/{carId}/accessories/upload")]
        public async Task<IActionResult> UploadCarAccessoriesDocuments(int carId, [FromForm] UploadCarAcessoriesDto accessoriesDto)
        {
            // Retrieve the car accessories based on the carId
            var car = await _dbContext.Cars.Include(c => c.CarAccessories).FirstOrDefaultAsync(c => c.CarId == carId);

            if (car == null)
            {
                return new NotFoundObjectResult("Car not found.");
            }

            if (car.CarAccessories == null)
            {
                return new NotFoundObjectResult("Car accessories not found.");
            }

            int carAccessoriesId = car.CarAccessories.CarAccessoriesId;

            return await _assignAccessoriesToCarService.UploadCarAccessoriesDocuments(carId, carAccessoriesId, accessoriesDto);
        }



    }
}
