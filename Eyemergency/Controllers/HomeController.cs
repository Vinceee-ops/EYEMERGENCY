using Eyemergency.Areas.Identity.Data;
using Eyemergency.Data;
using Eyemergency.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Eyemergency.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly EyemergencyDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger , EyemergencyDbContext context, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
        public async Task<IActionResult> Dashboard()
        {
            var user = await _userManager.GetUserAsync(User);

            ViewBag.VehicleCount = _context.Vehicle.Where(v => v.UserId == user.Id).Count();
            ViewBag.TicketCount = _context.EmergencyTickets.Where(v => v.AssignedToUserId == user.Id && v.Status != "Completed").Count();
            ViewBag.UserCount = _context.Users.Count();
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult VehicleCount()
        {
            List<Vehicle> Vehicles = _context.Vehicle.ToList();
            return View(Vehicles);
        }
    }
}
