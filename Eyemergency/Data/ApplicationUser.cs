using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Eyemergency.Models;

namespace Eyemergency.Areas.Identity.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    [PersonalData]
    //[Column(TypeName = "Nvarchar(100)")]
    //public string FirstName { get; set; }
    //[PersonalData]
    //[Column(TypeName = "Nvarchar(100)")]
    //public string LastName { get; set; }
    public bool IsApproved { get; set; } = false;

    [Display(Name = "Station Name")]
    public string StationName { get; set; }

    [Display(Name = "Station Type")]
    public string StationType { get; set; }  // "Fire", "Police", "Medical", etc.

    [Display(Name = "Station Location")]
    public string StationLocation { get; set; }

    public bool IsActiveStation { get; set; } = true;
    public virtual ICollection<EmergencyTicket> AssignedTickets { get; set; }


}

