using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SADPMRCarAPI.Services.TripApprovalService;

namespace SADPMRCarAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TripApprovalController : ControllerBase
    {
        private readonly ITripApprovalService _tripApprovalService;
        public TripApprovalController(ITripApprovalService tripApprovalService)
        {
            _tripApprovalService = tripApprovalService;
        }

        [HttpPost("trips/{tripId}/approveByManager")]
        public async Task<IActionResult> ApproveTripByManager(int tripId, int statusId)
        {
            return await _tripApprovalService.ApproveTripByManager(tripId, statusId);
        }

        [HttpPost("trips/{tripId}/approveByGeneralManager")]
        public async Task<IActionResult> ApproveTripByGeneralManager(int tripId, int statusId)
        {
            return await _tripApprovalService.ApproveTripByGeneralManager(tripId , statusId);
        }
    }

}

