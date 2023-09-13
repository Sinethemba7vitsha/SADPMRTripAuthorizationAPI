using SADPMRCarAPI.Model;

namespace SADPMRCarAPI.DTO.CarDto
{
    public class AddCarDto
    {
        public string? MakeOfTheCar { get; set; }
        public string? RegistrationOfTheCar { get; set; }
        public CarAccessories CarAccessories { get; set; }
    }
}
