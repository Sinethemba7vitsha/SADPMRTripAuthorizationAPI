using SADPMRCarAPI.Model;
using System.Text.Json.Serialization;

namespace SADPMRCarAPI.DTO.CarAccessoriesDto
{
    public class AddCarAccessoriesDto
    {
        public int OdometerDepart { get; set; }
        public int OdometerArrival { get; set; }

        public bool SpareWheel { get; set; }
        public bool Triangle { get; set; }
        public bool Jack { get; set; }
        public bool PullRod { get; set; }

        [JsonIgnore]
        public byte[]? PetrolReceiptData { get; set; }

        [JsonIgnore]
        public byte[]? TollGateReceiptData { get; set; }

     

    }
}
