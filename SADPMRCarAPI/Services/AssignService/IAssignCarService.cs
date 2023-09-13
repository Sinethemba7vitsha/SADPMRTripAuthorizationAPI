using Microsoft.AspNetCore.Mvc;

namespace SADPMRCarAPI.Services.AssignService
{
    public interface IAssignCarService
    {
        Task<IActionResult> AssignCarToTrip(int tripId, int carId);
    }
}

