using Microsoft.EntityFrameworkCore;
using SADPMRCarAPI.Data;
using SADPMRCarAPI.Model;

namespace SADPMRCarAPI.Services.CarModelService
{
    public interface ICarModelService
    {
        Task<IEnumerable<CarModel>> GetAllCarsAsync();
        Task<CarModel> GetCarByIdAsync(int id);
        Task<int> AddCarAsync(CarModel car);
        Task<bool> UpdateCarAsync(CarModel car);
        Task<bool> DeleteCarAsync(int id);
    }
}
