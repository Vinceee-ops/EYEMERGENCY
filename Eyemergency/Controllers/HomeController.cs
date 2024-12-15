using Eyemergency.Areas.Identity.Data;
using Eyemergency.Data;
using Eyemergency.Models;
using Microsoft.AspNetCore.Authorization;
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

        public HomeController(ILogger<HomeController> logger , EyemergencyDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
        public IActionResult Dashboard()
        {
            ViewBag.VehicleCount = _context.Vehicle.Count();
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
