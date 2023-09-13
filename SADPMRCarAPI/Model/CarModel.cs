using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SADPMRCarAPI.Model
{
    public class CarModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CarId { get; set; }
        public string? MakeOfTheCar { get; set; } 
        public string? RegistrationOfTheCar { get; set; }
        public CarServiceModel? CarService { get; set; }
        public CarAccessories? CarAccessories { get; set; } 
        public int DepartmentId { get; set; }   
        public List<TripModel> TripList { get; set; }
    }
}

