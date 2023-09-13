namespace SADPMRCarAPI.DTO.CarServiceDto
{
    public class UpdateCarServiceDto
    {
        public int CarServiceId { get; set; }
        public DateTime DateOfTheService { get; set; }
        public bool CarAvailability { get; set; }
    }
}
