// Controllers/Api/EmergencyApiController.cs
using Eyemergency.Data;
using Eyemergency.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Eyemergency.Controllers.Api
{
    [Route("api/emergency")]
    [ApiController]
    public class EmergencyApiController : ControllerBase
    {
        private readonly EyemergencyDbContext _context;

        public EmergencyApiController(EyemergencyDbContext context)
        {
            _context = context;
        }

        [HttpGet("check/{rfid}")]
        public async Task<IActionResult> CheckRfid(string rfid)
        {
            var vehicle = await _context.Vehicle
                .FirstOrDefaultAsync(v => v.TagId == rfid && v.IsActive);

            if (vehicle == null)
                return NotFound(new { Message = "Vehicle not found" });

            if (!vehicle.IsEmergencyVehicle)
                return Ok(new
                {
                    vehicle.Id,
                    vehicle.TagId,
                    vehicle.VehicleName,
                    IsEmergency = false
                });

            // Create emergency status
            var emergency = new EmergencyStatus
            {
                VehicleId = vehicle.Id,
                IsActive = true,
                ActivationTime = DateTime.Now,
                ExpirationTime = DateTime.Now.AddMinutes(2),
                TriggeredLane = vehicle.ApproachLane
            };

            _context.EmergencyStatus.Add(emergency);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                vehicle.Id,
                vehicle.TagId,
                vehicle.VehicleName,
                vehicle.ApproachLane,
                IsEmergency = true,
                Expiration = emergency.ExpirationTime
            });
        }

        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentEmergency()
        {
            var emergency = await _context.EmergencyStatus
                .Include(e => e.Vehicle)
                .Where(e => e.IsActive &&
                    (e.ExpirationTime == null || e.ExpirationTime > DateTime.Now))
                .OrderByDescending(e => e.ActivationTime)
                .FirstOrDefaultAsync();

            if (emergency == null)
                return Ok(new { Status = "NORMAL" });

            return Ok(new
            {
                Status = "EMERGENCY",
                Lane = emergency.TriggeredLane,
                Vehicle = emergency.Vehicle.VehicleName,
                Expiration = emergency.ExpirationTime
            });
        }
    }
}