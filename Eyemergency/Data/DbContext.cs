using Eyemergency.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Eyemergency.Models;

namespace Eyemergency.Data;

public class EyemergencyDbContext : IdentityDbContext<ApplicationUser>
{
    public EyemergencyDbContext(DbContextOptions<EyemergencyDbContext> options)
        : base(options)
    {
    }
    public DbSet<Vehicle> Vehicle { get; set; } 
}
