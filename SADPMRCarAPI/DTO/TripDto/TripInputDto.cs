using NSwag.Annotations;
using SADPMRCarAPI.Model;
using System.Text.Json.Serialization;

namespace SADPMRCarAPI.DTO.TripDto
{
    public class TripInputDto
    {
      
        public string? NameOfCoDriver { get; set; }
        public string? TimeOut { get; set; }
        public string? ReturnTime { get; set; }
        public string? Date { get; set; }
        public string? ReasonForTravel { get; set; }
        public string? Destination { get; set; }
        [JsonIgnore]
        public int? CarId { get; set; }

        [JsonIgnore]
        public int? StatusId { get; set; }

        [JsonIgnore]
        public string? UserEmail { get; set; }
    }

}
