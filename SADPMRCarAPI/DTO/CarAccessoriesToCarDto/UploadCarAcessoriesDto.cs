using System.Text.Json.Serialization;

namespace SADPMRCarAPI.DTO.CarAccessoriesDto
{
    public class UploadCarAcessoriesDto
    {
        public IFormFile? PetrolReceipt { get; set; }
        public IFormFile? TollGateReceipt { get; set; }        
    }
}
