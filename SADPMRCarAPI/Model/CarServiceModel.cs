using System.ComponentModel.DataAnnotations;

namespace SADPMRCarAPI.Model
{
    public class CarServiceModel
    {
        [Key]
        public int CarServiceId { get; set; }
        public DateTime DateOfTheService { get; set; }
        public bool CarAvailability { get; set; }
    }
}
