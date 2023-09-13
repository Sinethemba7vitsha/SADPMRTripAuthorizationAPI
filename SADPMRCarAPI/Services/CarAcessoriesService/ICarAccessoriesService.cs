using Microsoft.EntityFrameworkCore;
using SADPMRCarAPI.Data;
using SADPMRCarAPI.Model;

namespace SADPMRCarAPI.Services.CarAcessoriesService
{
    public interface ICarAccessoriesService
    {
        IEnumerable<CarAccessories> GetAll();
        CarAccessories GetById(int id);
        void Add(CarAccessories carAccessories);
        void Update(CarAccessories carAccessories);
        void Delete(int id);
    }

   
}
