
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SADPMRCarAPI.Data;
using SADPMRCarAPI.Model;
using SADPMRCarAPI.Services.AssignService;
using System.Security.Claims;

namespace SADPMRCarAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AssignCarController : ControllerBase
    {
        private readonly IAssignCarService _assignCarService;

        public AssignCarController(IAssignCarService assignCarService)
        {
            _assignCarService = assignCarService;
        }

        [HttpPost("{tripId}/assigncar")]
        public async Task<IActionResult> AssignCarToTrip(int tripId, [FromForm] int carId)
        {
            return await _assignCarService.AssignCarToTrip(tripId, carId);
        }
    }
}
