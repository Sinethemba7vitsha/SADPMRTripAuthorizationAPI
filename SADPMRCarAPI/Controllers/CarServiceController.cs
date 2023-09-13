using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SADPMRCarAPI.Model;
using SADPMRCarAPI.Services.CarService;

namespace SADPMRCarAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class CarServicesController : ControllerBase
        {
            private readonly ICarServiceService _repository;

            public CarServicesController(ICarServiceService repository)
            {
                _repository = repository;
            }

            [HttpGet("GetCarService")]
            public async Task<ActionResult<CarServiceModel>> GetCarService(int id)
            {
                var carService = await _repository.GetCarServiceAsync(id);

                if (carService == null)
                {
                    return NotFound();
                }

                return carService;
            }

            [HttpGet("GetListOfAllCarServices")]
            public async Task<ActionResult<IEnumerable<CarServiceModel>>> GetAllCarServices()
            {
                var carServices = await _repository.GetAllCarServicesAsync();

                return Ok(carServices);
            }

            [HttpPost("Add-CarService")]
            public async Task<ActionResult<CarServiceModel>> AddCarService(CarServiceModel carService)
            {
                await _repository.AddCarServiceAsync(carService);

                return CreatedAtAction(nameof(GetCarService), new { id = carService.CarServiceId }, carService);
            }

            [HttpPut("Update-CarService")]
            public async Task<IActionResult> UpdateCarService(int id, CarServiceModel carService)
            {
                if (id != carService.CarServiceId)
                {
                    return BadRequest();
                }

                await _repository.UpdateCarServiceAsync(carService);

                return NoContent();
            }

        }
    }

