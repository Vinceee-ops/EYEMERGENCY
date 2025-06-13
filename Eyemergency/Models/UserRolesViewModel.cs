namespace Eyemergency.Models
{
    public class UserRolesViewModel
    {
        public string UserId { get; set; }
        public string StationName { get; set; }
        public string Stationtype { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public List<string> Roles { get; set; }
    }
}
