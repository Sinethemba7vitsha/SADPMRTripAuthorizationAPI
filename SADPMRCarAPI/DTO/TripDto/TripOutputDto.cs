using NSwag.Annotations;
using SADPMRCarAPI.DTO.CarDto;
using SADPMRCarAPI.Model;
using System.Text.Json.Serialization;

namespace SADPMRCarAPI.DTO.TripDto
{
    public class TripOutputDto
    {
      
        public int TripId { get; set; } 
        public string? NameOfCoDriver { get; set; }
        public string TimeOut { get; set; }
        public string ReturnTime { get; set; }
        public string Date { get; set; }
        public string? ReasonForTravel { get; set; }
        public string? Destination { get; set; }

        [JsonIgnore]
        public int? CarId { get; set; }


        public int StatusId { get; set; }
        public string Status { get; set; }
        public DateTime StatusUpdatedDate { get; set; }

    }
}
