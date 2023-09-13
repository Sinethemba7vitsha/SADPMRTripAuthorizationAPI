using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SADPMRCarAPI.Data;
using SADPMRCarAPI.DTO.CarAccessoriesDto;
using SADPMRCarAPI.DTO.CarDto;
using SADPMRCarAPI.Model;
using SADPMRCarAPI.Services.CarAcessoriesService;

namespace SADPMRCarAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CarController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ICarAccessoriesService _carAccessoriesService;
        private readonly IMapper _mapper;

        public CarController(ApplicationDbContext context, ICarAccessoriesService carAccessoriesService, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _carAccessoriesService = carAccessoriesService;
        }

           [HttpGet("Get-All-Cars")]
            public ActionResult<List<CarModelDto>> GetCars()
            {
                var cars = _context.Cars
                    .Select(c => new CarModelDto
                    {
                        CarId = c.CarId,
                        MakeOfTheCar = c.MakeOfTheCar,
                        RegistrationOfTheCar = c.RegistrationOfTheCar,
                      
                    })
                    .ToList();

                return Ok(cars);
        }
 


        [HttpPut("Update-Car")]
        public async Task<IActionResult> PutCar(int id, CarModel car)
        {
            if (id != car.CarId)
            {
                return BadRequest();
            }

            _context.Entry(car).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("DeleteCar")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.CarId == id);
        }


        [HttpGet("GetById")]
        public CarAccessories GetById(int id)
        {
            return _context.CarAccessories.Find(id);
        }


        [HttpPost("cars/{carId}/accessories")]
        public async Task<IActionResult> AssignAccessoriesToCar(int carId, [FromForm] AddCarAccessoriesDto accessoriesDto)
        {
            var car = await _context.Cars.FindAsync(carId);
            if (car == null)
            {
                return NotFound("Car not found.");
            }

            // Check if the car is currently in service
            if (car.CarService != null)
            {
                return BadRequest("Car is currently in service and cannot be assigned accessories.");
            }

            var carAccessories = new CarAccessories
            {
                OdometerDepart = accessoriesDto.OdometerDepart,
                OdometerArrival = accessoriesDto.OdometerArrival,
                SpareWheel = accessoriesDto.SpareWheel,
                Triangle = accessoriesDto.Triangle,
                Jack = accessoriesDto.Jack,
                PullRod = accessoriesDto.PullRod,
                
            };

            car.CarAccessories = carAccessories;

            await _context.SaveChangesAsync();

            return Ok("Car accessories assigned successfully.");
        }




    }
}





