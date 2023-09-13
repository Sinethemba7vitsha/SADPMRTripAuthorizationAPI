using Microsoft.EntityFrameworkCore;
using SADPMRCarAPI.Data;
using SADPMRCarAPI.Model;

namespace SADPMRCarAPI.Services.CarService
{
    public interface ICarServiceService
    {
        Task<CarServiceModel> GetCarServiceAsync(int carServiceId);
        Task<IEnumerable<CarServiceModel>> GetAllCarServicesAsync();
        Task AddCarServiceAsync(CarServiceModel carService);
        Task UpdateCarServiceAsync(CarServiceModel carService);
        Task DeleteCarServiceAsync(CarServiceModel carService);
    }
}
