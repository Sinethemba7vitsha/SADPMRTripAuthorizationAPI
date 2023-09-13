using Microsoft.EntityFrameworkCore;
using SADPMRCarAPI.Data;
using SADPMRCarAPI.Model;

namespace SADPMRCarAPI.Services.CarModelService
{
    public class CarRepository : ICarModelService
    {
        private readonly ApplicationDbContext _context;

        public CarRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CarModel>> GetAllCarsAsync()
        {
            return await _context.Cars.ToListAsync();
        }

        public async Task<CarModel> GetCarByIdAsync(int id)
        {
            return await _context.Cars.FindAsync(id);
        }

        public async Task<int> AddCarAsync(CarModel car)
        {
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();
            return car.CarId;
        }

        public async Task<bool> UpdateCarAsync(CarModel car)
        {
            var existingCar = await _context.Cars.FindAsync(car.CarId);
            if (existingCar == null)
            {
                return false;
            }

            existingCar.CarService.DateOfTheService = car.CarService.DateOfTheService;
            existingCar.CarService.CarAvailability = car.CarService.CarAvailability;


            _context.Entry(existingCar).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<bool> DeleteCarAsync(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return false;
            }

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
