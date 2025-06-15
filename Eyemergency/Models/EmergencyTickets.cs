using Eyemergency.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Eyemergency.Models
{
    public class EmergencyTicket
    {
        [Key]
        public int TicketId { get; set; }

        [Required]
        [Display(Name = "Caller Name")]
        public string? CallerName { get; set; }

        [Required]
        [Display(Name = "Caller Phone")]
        [Phone]
        public string CallerPhone { get; set; }

        [Required]
        [Display(Name = "Emergency Type")]
        public string EmergencyType { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        [Range(1, 3)]
        public int Priority { get; set; } = 2;

        public string Status { get; set; } = "Pending";
        [Display(Name = "Status")]
        public TicketStatus TicketStatuses { get; set; } = TicketStatus.Pending;

        public enum TicketStatus
        {
            Pending,
            Completed,
            InProgress
        }

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Completed At")]
        public DateTime CompletedAt { get; set; } = DateTime.Now;

        [Display(Name = "Assigned To")]
        public string? AssignedToUserId { get; set; }  // Changed to string for Identity UserId

        [Display(Name = "Assigned To")] 
        public int? AssignedToVehicleId { get; set; }
        public Vehicle AssignedToVehicle { get; set; }
        public string? Notes { get; set; }

        // Navigation property to Identity User
        public virtual ApplicationUser AssignedToUser { get; set; }

    }
}

