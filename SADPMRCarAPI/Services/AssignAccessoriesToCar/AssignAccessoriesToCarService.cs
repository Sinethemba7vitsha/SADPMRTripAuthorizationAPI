using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SADPMRCarAPI.Data;
using SADPMRCarAPI.DTO.CarAccessoriesDto;
using SADPMRCarAPI.Model;
using System.Security.Claims;

namespace SADPMRCarAPI.Services.AssignAccessoriesToCar
{
    public class AssignAccessoriesToCarService : IAssignAccessoriesToCarService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUserIdentity> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AssignAccessoriesToCarService(ApplicationDbContext context, UserManager<ApplicationUserIdentity> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
           
        }
        private string GetUserId() => _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        public async Task<IActionResult> AssignAccessoriesToCar(int carId, AddCarAccessoriesDto accessoriesDto)
        {
            // Find the car by its ID
            var car = await _context.Cars.FindAsync(carId);
            if (car == null)
            {
                return new NotFoundObjectResult("Car not found.");
            }

       
            // Check if the car has accessories assigned
            if (car.CarAccessories == null)
            {
               car.CarAccessories = new CarAccessories();
              
            }

            // Retrieve the currently logged-in user
            var currentUser = await _context.Users.FirstOrDefaultAsync(c => c.Id == GetUserId());
            if (currentUser == null)
            {
                return new UnauthorizedResult();
            }

            if (car.DepartmentId != currentUser.DepartmentId)
            {
                return new BadRequestObjectResult("Sorry! You cannot assign a car to a different department.");
            }

            // Update the car accessories with the document data
            car.CarAccessories.PetrolReceiptData = null;
            car.CarAccessories.TollGateReceiptData = null;

            // Save the changes to the database
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return new ObjectResult($"An error occurred while saving the entity changes: {ex.InnerException?.Message}")
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }

            return new OkObjectResult("Car accessories updated successfully.");
        }

        public async Task<IActionResult> UploadCarAccessoriesDocuments(int carId, int CarAccessoriesId ,UploadCarAcessoriesDto accessoriesDto)
        {
            // Find the car by its ID
            var car = await _context.Cars.FindAsync(carId);
            if (car == null)
            {
                return new NotFoundObjectResult("Car not found.");
            }

            var carAccess = await _context.CarAccessories.FindAsync(CarAccessoriesId);

            if(carAccess == null)
            {
                return new NotFoundObjectResult("CarAcessories Not Found");
            }

            // Check if the car has accessories assigned
            if (car.CarAccessories == null)
            {
                car.CarAccessories = new CarAccessories();
            }

            // Retrieve the currently logged-in user
            var currentUser = await _context.Users.FirstOrDefaultAsync(c => c.Id == GetUserId());
            if (currentUser == null)
            {
                return new UnauthorizedResult();
            }

            if (car.DepartmentId != currentUser.DepartmentId)
            {
                return new BadRequestObjectResult("Sorry! You cannot assign a car to a different department.");
            }

            // Get the PetrolReceipt file data as a byte array
            byte[]? petrolReceiptData = await GetFileDataAsByteArray(accessoriesDto.PetrolReceipt);

            // Get the TollGateReceipt file data as a byte array
            byte[]? tollGateReceiptData = await GetFileDataAsByteArray(accessoriesDto.TollGateReceipt);

            // Update the car accessories with the document data
            car.CarAccessories.PetrolReceiptData = petrolReceiptData;
            car.CarAccessories.TollGateReceiptData = tollGateReceiptData;

            await _context.SaveChangesAsync();

            return new OkObjectResult("Supporting documents updated successfully.");
        }


        private static async Task<byte[]?> GetFileDataAsByteArray(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            var storagePath = Path.Combine("Resource", "Documents");
            var fileName = Path.GetFileName(file.FileName);
            var filePath = Path.Combine(storagePath, fileName);

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);

                // Write the memory stream data to the specified file path
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    await memoryStream.CopyToAsync(fileStream);
                }
            }

            return File.ReadAllBytes(filePath);
        }
    }
}
