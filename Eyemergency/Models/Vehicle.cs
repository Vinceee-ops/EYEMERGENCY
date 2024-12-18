using System.ComponentModel.DataAnnotations;

namespace Eyemergency.Models
{
    public class Vehicle
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "RFID Tag ID")]
        public string TagId { get; set; }

        [Required]
        [Display(Name = "Vehicle Name")]
        public string VehicleName { get; set; }

        [Required]
        [Display(Name = "Vehicle Type")]
        public string VehicleType { get; set; }

        [Display(Name = "Registration Date")]
        public DateTime RegisteredAt { get; set; } = DateTime.Now;

    }
}
