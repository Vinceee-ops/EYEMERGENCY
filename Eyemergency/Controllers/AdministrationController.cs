using Eyemergency.Data;
using Microsoft.AspNetCore.Mvc;
using Eyemergency.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Eyemergency.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Eyemergency.Controllers
{
    [Authorize(Roles = "Regional, Admin")]
    public class AdministrationController : Controller
    {
        private readonly ILogger<AdministrationController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdministrationController(ILogger<AdministrationController> logger, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Users()
        {
            var users = _userManager.Users.ToList();
            var userRolesViewModel = new List<UserRolesViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userRolesViewModel.Add(new UserRolesViewModel
                {
                    UserId = user.Id,
                    StationName = user.StationName,
                    Stationtype = user.StationType,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Roles = roles.ToList()
                });
            }

            return View(userRolesViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> ApproveUser(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            // Approve the user
            user.IsApproved = true;
            await _userManager.UpdateAsync(user);

            // Remove existing roles (optional)
            var existingRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, existingRoles);

            // Assign the selected role to the user
            await _userManager.AddToRoleAsync(user, role);

            // Add the IsApproved claim
            await _userManager.AddClaimAsync(user, new Claim("IsApproved", "True"));

            // Redirect to the Users action to re-fetch the data
            return RedirectToAction("Users");
        }
    }

}
