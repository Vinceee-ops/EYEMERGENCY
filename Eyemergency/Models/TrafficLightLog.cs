using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Eyemergency.Models
{
    public class TrafficLightLog
    {
        [Key]
        public int Id { get; set; }

        public DateTime ChangeTime { get; set; } = DateTime.Now;

        [StringLength(20)]
        public string PreviousState { get; set; } // "NORMAL", "EMERGENCY_NORTH", etc.

        [StringLength(20)]
        public string NewState { get; set; }

        [ForeignKey("Vehicle")]
        public int? TriggeredByVehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
    }
}