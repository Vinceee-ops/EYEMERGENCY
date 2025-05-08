using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Eyemergency.Data;
using Eyemergency.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);

// Get the connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddHostedService<SerialPortService>();

// Add DbContext with Identity
builder.Services.AddDbContext<EyemergencyDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add Identity with Roles
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<EyemergencyDbContext>()
.AddDefaultUI()
.AddDefaultTokenProviders();

// Add services to the container
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApprovedUser", policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim(c => c.Type == "IsApproved" && c.Value == "True")));
});

var app = builder.Build();

// Seed Roles into the Database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedRoles(services);
}

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Ensure Authentication is called before Authorization
app.UseAuthorization();
// Minimal API endpoint for emergency system
//app.MapGet("/api/emergency/check/{rfid}", async (string rfid, EyemergencyDbContext db) =>
//{
//    var vehicle = await db.Vehicle
//        .FirstOrDefaultAsync(v => v.TagId == rfid && v.IsActive && v.IsEmergencyVehicle);

//    if (vehicle == null)
//        return Results.NotFound();

//    return Results.Ok(new
//    {
//        vehicle.TagId,
//        vehicle.ApproachLane,
//        vehicle.VehicleName
//    });
//});

// Add authentication if needed
// app.MapGet("/api/emergency/check/{rfid}", ...).RequireAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

// Seed Roles Method
async Task SeedRoles(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    string[] roleNames = { "Admin", "User", "Manager" };

    foreach (var roleName in roleNames)
    {
        var roleExist = await roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            // Create the role if it doesn't exist
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}