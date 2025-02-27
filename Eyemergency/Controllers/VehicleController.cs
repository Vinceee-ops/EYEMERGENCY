using Eyemergency.Areas.Identity.Data;
using Eyemergency.Data;
using Eyemergency.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace Eyemergency.Controllers
{
    public class VehicleController : Controller
    {
        private readonly ILogger<VehicleController> _logger;
        private readonly EyemergencyDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public VehicleController(ILogger<VehicleController> logger, EyemergencyDbContext context, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        // GET: Vehicle
        public ActionResult Index()
        {
            var vehicles = _context.Vehicle.ToList();
            return View(vehicles);
        }

        // GET: Vehicle/Create
        public async Task<ActionResult> Create()    
        {
            // Get the currently logged-in user
            var user = await _userManager.GetUserAsync(User);

            // Create a new Vehicle model and set the UserId
            var vehicle = new Vehicle
            {
                UserId = user?.Id // Set the UserId to the currently logged-in user's Id
            };

            return View(vehicle);
        }

        // POST: Vehicle/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                // Check if TagId already exists
                if (_context.Vehicle.Any(v => v.TagId == vehicle.TagId))
                {
                    ModelState.AddModelError("TagId", "This RFID Tag is already registered.");
                    return View(vehicle);
                }

                _context.Vehicle.Add(vehicle);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vehicle);
        }

        // GET: Vehicle/Edit/{id}
        public ActionResult Edit(int id)
        {
            var vehicle = _context.Vehicle.Find(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            return View(vehicle);
        }

        // POST: Vehicle/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(vehicle).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vehicle);
        }

        // GET: Vehicle/Delete/{id}
        public ActionResult Delete(int id)
        {
            var vehicle = _context.Vehicle.Find(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            return View(vehicle);
        }

        // POST: Vehicle/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var vehicle = _context.Vehicle.Find(id);
            if (vehicle != null)
            {
                _context.Vehicle.Remove(vehicle);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
