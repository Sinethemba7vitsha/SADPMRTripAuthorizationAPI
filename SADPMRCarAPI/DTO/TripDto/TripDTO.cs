using SADPMRCarAPI.DTO.CarDto;
using SADPMRCarAPI.DTO.CarServiceDto;
using SADPMRCarAPI.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace SADPMRCarAPI.DTO.TripDto
{
    public class TripDTO
    {
        public int TripId { get; set; }
        public DateTime DateDepart { get; set; }
        public DateTime DateArrive { get; set; }
        public DateTime DateOfTheDay { get; set; }
        public string ReasonForTravel { get; set; }
        public string Destination { get; set; }
        public int CarId { get; set; }
        public CarModelDto CarModel { get; set; }
    }
}
