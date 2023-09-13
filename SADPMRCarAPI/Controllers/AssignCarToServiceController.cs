using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SADPMRCarAPI.Data;
using SADPMRCarAPI.DTO.CarServiceDto;
using SADPMRCarAPI.Model;
using SADPMRCarAPI.Services.AssignCarToServiceService;

namespace SADPMRCarAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AssignCarToServiceController : ControllerBase
    {
        private readonly IAssignCarToService _assignCarToService;

        public AssignCarToServiceController(IAssignCarToService assignCarToService)
        {
            _assignCarToService = assignCarToService;
        }

        [HttpPost("cars/{carId}/service")]
        public async Task<IActionResult> PutCarInService(int carId , AddCarServiceDto addCarServiceDto)
        {
            return await _assignCarToService.PutCarInService(carId , addCarServiceDto);
        }

        [HttpPost("cars/{carId}/service/end")]
        public async Task<IActionResult> EndCarService(int carId)
        {
            return await _assignCarToService.EndCarService(carId);
        }
    }
}
