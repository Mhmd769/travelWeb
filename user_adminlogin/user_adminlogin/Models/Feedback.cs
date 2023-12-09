using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using user_adminlogin.Data;

namespace user_adminlogin.Models
{
    public class Feedback
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(250)]
        public string feedback_details { get; set; }
        [Required]
        public int Rate { get; set; }
        // Navigation properties for one-to-many relationship with Flight and ApplicationUser
        public int FlightId { get; set; }
        public Flight Flight { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
