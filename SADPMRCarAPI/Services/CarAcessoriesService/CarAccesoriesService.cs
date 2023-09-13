using Microsoft.EntityFrameworkCore;
using SADPMRCarAPI.Data;
using SADPMRCarAPI.Model;

namespace SADPMRCarAPI.Services.CarAcessoriesService
{
    public class CarAccessoriesRepository : ICarAccessoriesService
    {
        private readonly ApplicationDbContext _dbContext;

        public CarAccessoriesRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<CarAccessories> GetAll()
        {
            return _dbContext.CarAccessories;
        }

        public CarAccessories GetById(int id)
        {
            return _dbContext.CarAccessories.Find(id);
        }

        public void Add(CarAccessories carAccessories)
        {
            _dbContext.CarAccessories.Add(carAccessories);
            _dbContext.SaveChanges();
        }

        public void Update(CarAccessories carAccessories)
        {
            _dbContext.Entry(carAccessories).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var carAccessories = _dbContext.CarAccessories.Find(id);
            _dbContext.CarAccessories.Remove(carAccessories);
            _dbContext.SaveChanges();
        }
    }
}
