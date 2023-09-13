using Microsoft.AspNetCore.Mvc;
using SADPMRCarAPI.DTO.CarAccessoriesDto;

namespace SADPMRCarAPI.Services.AssignAccessoriesToCar
{
    public interface IAssignAccessoriesToCarService
    {
        Task<IActionResult> AssignAccessoriesToCar(int carId, AddCarAccessoriesDto accessoriesDto);
        Task<IActionResult> UploadCarAccessoriesDocuments(int carId, int CarAccessoriesId, UploadCarAcessoriesDto accessoriesDto);
    }
}

