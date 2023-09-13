using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SADPMRCarAPI.Model
{    
    public class CarAccessories
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CarAccessoriesId { get; set; }
        public int OdometerDepart { get; set; }
        public int OdometerArrival { get; set; }
        public bool SpareWheel { get; set; }
        public bool Triangle { get; set; }
        public bool Jack { get; set; }
        public bool PullRod { get; set; }
        public byte[]? PetrolReceiptData { get; set; }
        public byte[]? TollGateReceiptData { get; set; }
    }

}








