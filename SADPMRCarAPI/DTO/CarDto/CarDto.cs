namespace SADPMRCarAPI.DTO.CarDto
{
    public class CarDto
    {
        public int CarId { get; set; }
        public string? MakeOfTheCar { get; set; }
        public string? RegistrationOfTheCar { get; set; }
        public int OdometerDepart { get; set; }
        public int OdometerArrival { get; set; }
    }
}
