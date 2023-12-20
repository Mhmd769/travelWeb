using Microsoft.AspNetCore.Identity;
using user_adminlogin.Models;

namespace user_adminlogin.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string? name { get; set; }
        public string? ProfilePicture { get; set; }
        public List<Booking> UserFlights { get; set; }
        public List<Feedback> Feedbacks { get; set; }

    }
}
