using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace user_adminlogin.Models
{
    public class Package
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(256)]
        public string Package_details { get; set; }
        [Required]
        public int Price {  get; set; }

        // Navigation property for the one-to-many relationship
        public int FlightId { get; set; }
        public Flight Flight { get; set; }

    }
}
