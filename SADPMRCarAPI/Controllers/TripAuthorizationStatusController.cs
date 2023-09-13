using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SADPMRCarAPI.DTO.TripAuthorizationStatusDTO;
using SADPMRCarAPI.Services.TripAuthorizationStatusService;

namespace SADPMRCarAPI.Controllers
{
    [ApiController]
    [Route("api/tripauthorizationstatus")]
    public class TripAuthorizationStatusController : ControllerBase
    {
        private readonly ITripAuthorizationStatusRepository _repository;

        public TripAuthorizationStatusController(ITripAuthorizationStatusRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TripAuthorizationStatusDTO>> GetById(int id)
        {
            var tripAuthorizationStatus = await _repository.GetById(id);

            if (tripAuthorizationStatus == null)
            {
                return NotFound();
            }

            return Ok(tripAuthorizationStatus);
        }

        [HttpGet]
        public async Task<ActionResult<List<TripAuthorizationStatusDTO>>> GetAll()
        {
            var tripAuthorizationStatusList = await _repository.GetAll();
            return Ok(tripAuthorizationStatusList);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(TripAuthorizationStatusDTO tripAuthorizationStatusDTO)
        {
            var createdId = await _repository.Create(tripAuthorizationStatusDTO);
            return CreatedAtAction(nameof(GetById), new { id = createdId }, createdId);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TripAuthorizationStatusDTO tripAuthorizationStatusDTO)
        {
            if (id != tripAuthorizationStatusDTO.Id)
            {
                return BadRequest();
            }

            await _repository.Update(tripAuthorizationStatusDTO);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.Delete(id);
            return NoContent();
        }
    }

}
