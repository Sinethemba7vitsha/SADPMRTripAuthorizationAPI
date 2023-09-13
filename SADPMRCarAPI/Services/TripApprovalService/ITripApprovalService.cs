using Microsoft.AspNetCore.Mvc;

namespace SADPMRCarAPI.Services.TripApprovalService
{
    public interface ITripApprovalService
    {
        Task<IActionResult> ApproveTripByManager(int tripId, int statusId);
        Task<IActionResult> ApproveTripByGeneralManager(int tripId , int statusId);
    }
}
