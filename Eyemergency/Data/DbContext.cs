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
    public DbSet<EmergencyStatus> EmergencyStatus { get; set; }
    public DbSet<TrafficLightLog> TrafficLightLogs { get; set; }
    public DbSet<EmergencyTicket> EmergencyTickets { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // User relationship (which you already have)
        builder.Entity<EmergencyTicket>()
            .HasOne(t => t.AssignedToUser)
            .WithMany(u => u.AssignedTickets)
            .HasForeignKey(t => t.AssignedToUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Vehicle relationship
        builder.Entity<EmergencyTicket>()
            .HasOne(t => t.AssignedToVehicle)
            .WithMany(v => v.AssignedTickets)
            .HasForeignKey(t => t.AssignedToVehicleId)
            .OnDelete(DeleteBehavior.Restrict); // Or Cascade based on your needs
    }
}
