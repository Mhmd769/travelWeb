using user_adminlogin.Data;

namespace user_adminlogin.Models
{
    public class Booking
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int FlightId { get; set; }
        public Flight Flight { get; set; }
        
        public DateTime BookDate { get; set; }
    }
}
