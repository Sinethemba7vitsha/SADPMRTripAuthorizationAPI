using Microsoft.AspNetCore.Mvc;
using SADPMRCarAPI.DTO.CarServiceDto;

namespace SADPMRCarAPI.Services.AssignCarToServiceService
{
    public interface IAssignCarToService
    {
        Task<IActionResult> PutCarInService(int carId , AddCarServiceDto addCarServiceDto);
        Task<IActionResult> EndCarService(int carId);
    }

}
