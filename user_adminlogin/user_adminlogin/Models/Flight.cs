using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace user_adminlogin.Models
{
    public class Flight
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(256)]
        public string flight_Name { get; set; }
        public string Destenation { get; set; }
        [Required]
        [StringLength(256)]
        public string Departure { get; set; }
        public string? Arrival_time { get; set; }
        public string? Departure_time { get; set; }

        public DateTime? flight_date { get; set; }

        // Navigation property for the one-to-many relationship
        public List<Package>? Packages { get; set; }
        public List<Feedback>? Feedbacks { get; set; }
        public List<Booking>? UserFlights { get; set; }
    }
}
