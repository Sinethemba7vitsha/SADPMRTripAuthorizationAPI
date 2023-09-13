using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SADPMRCarAPI.DTO.TripDto;
using SADPMRCarAPI.Services.TripService;
using System.Security.Claims;

namespace SADPMRCarAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class TripsController : ControllerBase
    {
        private readonly ITripService _tripService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TripsController(ITripService tripService, IHttpContextAccessor httpContextAccessor)
        {
            _tripService = tripService;
            _httpContextAccessor = httpContextAccessor;
        }




        [HttpPost("Create-Trip")]
        public IActionResult CreateTrip(TripInputDto tripInputDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Retrieve the email of the currently logged-in user email
            var userEmail = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
           
            var createdTripDto = _tripService.CreateTrip(userEmail,tripInputDto);

            return CreatedAtAction(nameof(GetTrip), new { id = createdTripDto.TripId }, createdTripDto);
        }

        [HttpGet("Get-Trip/{id}")]
        public IActionResult GetTrip(int id)
        {
            var tripDto = _tripService.GetTrip(id);

            if (tripDto == null)
            {
                return NotFound();
            }

            return Ok(tripDto);
        }

        [HttpPut("Update-Trip/{id}")]
        public IActionResult UpdateTrip(int id, TripInputDto tripInputDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedTripDto = _tripService.UpdateTrip(id, tripInputDto);

            if (updatedTripDto == null)
            {
                return NotFound();
            }

            return Ok(updatedTripDto);
        }

        [HttpDelete("Delete-Trip/{id}")]
        public IActionResult DeleteTrip(int id)
        {
            _tripService.DeleteTrip(id);

            return NoContent();
        }

    }
}