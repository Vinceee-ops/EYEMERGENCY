// Models/EmergencyStatus.cs
using System;

namespace Eyemergency.Models
{
    public class EmergencyStatus
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
        public bool IsActive { get; set; }
        public DateTime ActivationTime { get; set; } = DateTime.Now;
        public DateTime? ExpirationTime { get; set; }
        public string TriggeredLane { get; set; }
    }
}