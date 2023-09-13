namespace SADPMRCarAPI.DTO.CarAccessoriesDto
{
    public class UpdateCarAccessoriesDto
    {
        public int CarAccessoriesId { get; set; }
        public int OdometerDepart { get; set; }
        public int OdometerArrival { get; set; }
        public bool SpareWheel { get; set; }
        public bool Triangle { get; set; }
        public bool Jack { get; set; }
        public bool PullRod { get; set; }     
    }
}
