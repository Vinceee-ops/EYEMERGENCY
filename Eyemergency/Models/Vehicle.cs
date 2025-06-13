using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eyemergency.Models
{
    public class Vehicle
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "User ID")]
        public string UserId { get; set; }

        [Required]
        [Display(Name = "RFID Tag ID")]
        [StringLength(50)]
        public string TagId { get; set; }

        [Required]
        [Display(Name = "Vehicle Name")]
        public string VehicleName { get; set; }

        [Required]
        [Display(Name = "Vehicle Type")]
        public string VehicleType { get; set; }

        [Display(Name = "Registered Date")]
        public DateTime RegisteredAt { get; set; } = DateTime.Now;

        // Emergency system properties
        [Display(Name = "Emergency Vehicle")]
        public bool IsEmergencyVehicle { get; set; }

        [Display(Name = "Approach Lane")]
        public string ApproachLane { get; set; } = "NORTH";

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;
        public virtual ICollection<EmergencyTicket> AssignedTickets { get; set; }

    }
}