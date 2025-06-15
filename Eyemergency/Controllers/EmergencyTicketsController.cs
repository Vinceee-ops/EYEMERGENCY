using Eyemergency.Areas.Identity.Data;
using Eyemergency.Data;
using Eyemergency.Migrations;
using Eyemergency.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using static Eyemergency.Models.EmergencyTicket;

namespace Eyemergency.Controllers
{
    [Authorize]
    public class EmergencyTicketsController : Controller
    {
        private readonly EyemergencyDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EmergencyTicketsController(
            EyemergencyDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: EmergencyTickets
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            var EmergencyTickets = await _context.EmergencyTickets
                .Include(t => t.AssignedToUser)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
            return View(EmergencyTickets);
        }
        // GET: EmergencyTickets
        [Authorize]
        public async Task<IActionResult> StationTickets()
        {
            var user = await _userManager.GetUserAsync(User);

            var emergencyTickets = await _context.EmergencyTickets
                .Include(t => t.AssignedToUser)
                .Where(t => t.AssignedToUserId == user.Id)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
            var availableVehicles = await _context.Vehicle
            .Where(v => v.UserId == user.Id && v.IsActive)
            .ToListAsync();

            ViewBag.AvailableVehicles = availableVehicles;
            return View(emergencyTickets);
        }

        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var ticket = await _context.EmergencyTickets
                .Include(t => t.AssignedToUser)
                .Include(t => t.AssignedToVehicle)
                .FirstOrDefaultAsync(t => t.TicketId == id);

            if (ticket == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            var availableVehicles = await _context.Vehicle
                .Where(v => v.UserId == user.Id && v.IsActive)
                .Select(v => new
                {
                    Vehicle = v,
                    HasPendingTickets = v.AssignedTickets.Any(t => t.Status != "Completed")
                })
                .Where(x => !x.HasPendingTickets) // Only vehicles with no pending tickets
                .Select(x => x.Vehicle)
                .ToListAsync();

            ViewBag.AvailableVehicles = availableVehicles;
            return View(ticket);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateStatus(int id, int vehicleId, EmergencyTicket.TicketStatus status)
        {
            var ticket = await _context.EmergencyTickets.FindAsync(id);
            var vehicle = await _context.Vehicle.FindAsync(vehicleId);
            if (ticket == null)
            {
                return NotFound();
            }
            ticket.CompletedAt = DateTime.Now;
            ticket.Status = status.ToString();
            vehicle.IsEmergencyVehicle = false;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(StationTickets));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AssignVehicle(int id, int vehicleId)
        {
            var ticket = await _context.EmergencyTickets.FindAsync(id);
            var vehicle = await _context.Vehicle.FindAsync(vehicleId);
            if (ticket == null)
            {
                return NotFound();
            }
            vehicle.IsEmergencyVehicle = true;
            ticket.AssignedToVehicleId = vehicleId;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(StationTickets));
        }
        // GET: EmergencyTickets/Create
        public IActionResult Create()
        {
            ViewBag.EmergencyTypes = new List<string> { "Medical", "Fire", "Police", "Traffic", "Other" };
            return View();
        }

        // POST: EmergencyTickets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmergencyTicket emergencyTicket)
        {

            emergencyTicket.CreatedAt = DateTime.Now;

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser != null && await _userManager.IsInRoleAsync(currentUser, "Regional"))
            {
                emergencyTicket.Status = "Pending";
            }
            else
            {
                emergencyTicket.Status = "Owned";
                emergencyTicket.AssignedToUserId = currentUser.Id;
            }
            _context.Add(emergencyTicket);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Emergency ticket created successfully!";
            return RedirectToAction(nameof(Index));

        }


        // GET: EmergencyTickets/Assign/5
        [Authorize(Roles = "Regional")]
        public async Task<IActionResult> Assign(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emergencyTicket = await _context.EmergencyTickets.FindAsync(id);
            if (emergencyTicket == null)
            {
                return NotFound();
            }

            // Get users with station roles that match the emergency type
            var stationUsers = await _userManager.Users
                .Where(u => u.StationType == emergencyTicket.EmergencyType && u.IsActiveStation)
                .ToListAsync();

            ViewBag.StationUsers = stationUsers;
            return View(emergencyTicket);
        }

        // POST: EmergencyTickets/Assign/5
        [HttpPost]
        [Authorize(Roles = "Regional")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign(int id, [Bind("TicketId,AssignedToUserId,Notes")] EmergencyTicket emergencyTicket)
        {
            if (id != emergencyTicket.TicketId)
            {
                return NotFound();
            }

            var ticketToUpdate = await _context.EmergencyTickets.FindAsync(id);
            if (ticketToUpdate == null)
            {
                return NotFound();
            }
            ticketToUpdate.AssignedToUserId = emergencyTicket.AssignedToUserId;
            ticketToUpdate.Notes = emergencyTicket.Notes;
            ticketToUpdate.Status = "Assigned";

            _context.Update(ticketToUpdate);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool EmergencyTicketExists(int id)
        {
            return _context.EmergencyTickets.Any(e => e.TicketId == id);
        }
    }
}
