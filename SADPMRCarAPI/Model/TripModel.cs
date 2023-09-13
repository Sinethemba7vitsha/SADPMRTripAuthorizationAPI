using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SADPMRCarAPI.Model
{
    public class TripModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       
        public int TripId { get; set; }

        public string? UserEmail { get; set; }
        public string? NameOfCoDriver { get; set; }
        public DateTime TimeOut { get; set; }
        public DateTime ReturnTime { get; set; }
        public DateTime Date { get; set; }
        public string? ReasonForTravel { get; set; }
        public string? Destination { get; set; }
        public DateTime StatusUpdatedDate { get; set; }
        public bool IsApprovedByManager { get; set; }
        public bool IsApprovedByGeneralManager { get; set; }
        public string? ManagerApprovalUser { get; set; }
        public string? GeneralManagerApprovalUser {get; set;}

       

        [NotMapped] 
        public string TimeOutString => TimeOut.ToString("HH:mm:ss");

        [NotMapped] 
        public string ReturnTimeString => ReturnTime.ToString("HH:mm:ss");

        [NotMapped] 
        public string DateString => Date.ToString("yyyy-MM-dd");
        public int? CarId { get; set; }
        public int StatusId { get; set; }   
        public virtual Status? Status { get; set; }
        public CarModel? Car { get; internal set; }
    }
}
