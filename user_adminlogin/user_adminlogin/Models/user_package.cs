using user_adminlogin.Data;

namespace user_adminlogin.Models
{
    public class user_package
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int PackageId { get; set; }
        public Package package { get; set; }
    }
}
