using Microsoft.EntityFrameworkCore;
using SADPMRCarAPI.Data;
using SADPMRCarAPI.Model;

namespace SADPMRCarAPI.Services.CarService
{
    public class CarServiceRepository : ICarServiceService
    {
        private readonly ApplicationDbContext _dbContext;

        public CarServiceRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CarServiceModel> GetCarServiceAsync(int carServiceId)
        {
            return await _dbContext.CarServices.FindAsync(carServiceId);
        }

        public async Task<IEnumerable<CarServiceModel>> GetAllCarServicesAsync()
        {
            return await _dbContext.CarServices.ToListAsync();
        }

        public async Task AddCarServiceAsync(CarServiceModel carService)
        {

            await _dbContext.CarServices.AddAsync(carService);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateCarServiceAsync(CarServiceModel carService)
        {
            _dbContext.Entry(carService).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteCarServiceAsync(CarServiceModel carService)
        {
            _dbContext.CarServices.Remove(carService);
            await _dbContext.SaveChangesAsync();
        }
    }
}
