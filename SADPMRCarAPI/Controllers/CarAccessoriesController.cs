using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SADPMRCarAPI.Model;
using SADPMRCarAPI.Services.CarAcessoriesService;

namespace SADPMRCarAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CarAccessoriesController : ControllerBase
    {
        private readonly ICarAccessoriesService _repository;

        public CarAccessoriesController(ICarAccessoriesService repository)
        {
            _repository = repository;
        }



        [HttpGet("GetAllAccessories")]
        public IEnumerable<CarAccessories> GetAll()
        {
            return _repository.GetAll();
        }

        [HttpGet("{id}")]
        public ActionResult<CarAccessories> GetById(int id)
        {
            var carAccessories = _repository.GetById(id);

            if (carAccessories == null)
            {
                return NotFound();
            }

            return carAccessories;
        }

        [HttpPost]
        public IActionResult Create(CarAccessories carAccessories)
        {
            _repository.Add(carAccessories);
            return CreatedAtAction(nameof(GetById), new { id = carAccessories.CarAccessoriesId }, carAccessories);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CarAccessories carAccessories)
        {
            if (id != carAccessories.CarAccessoriesId)
            {
                return BadRequest();
            }

            _repository.Update(carAccessories);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _repository.Delete(id);
            return NoContent();
        }
    }
}
