using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net.Security;
using System.Security.Claims;
using user_adminlogin.Data;
using user_adminlogin.Models;

namespace Flight_dbproject.Controllers
{
    [Authorize]
    public class PackageController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;

        public PackageController(UserManager<ApplicationUser> userManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }
        public IActionResult Index()
        {
            var uId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var booked = from b in _db.user_Packages
                         where b.UserId == uId
                         select b.package;
                var package = from row in _db.Packages
                              select row;
                IEnumerable<Package> PackageList = package.Except(booked).ToList();
                return View(PackageList);
            
        }
        public async Task<IActionResult> bookNow(int id)
        {
            var uId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                ApplicationUser user = await _userManager.FindByIdAsync(uId);
                var package = _db.Packages.Find(id);
                var userPackages = new user_package
                {
                    UserId = uId,
                    User = user,
                    PackageId = id,
                    package = package
                };
                _db.user_Packages.Add(userPackages);
                _db.SaveChanges();
                return RedirectToAction("BookPackage");
            }
        public ActionResult BookPackage()
        {

            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var bookings = from row in _db.user_Packages
                           where row.UserId == id
                           select row.package;
            List<Package> bookingList = bookings.ToList();
            return View(bookingList);
        }
        public ActionResult cancelPackage(int id)
        {
            var uId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var booking = _db.user_Packages.FirstOrDefault(b => b.UserId == uId && b.PackageId == id);
            Console.WriteLine("********" + booking + "********");
            if (booking != null)
            {
                _db.user_Packages.Remove(booking);
                _db.SaveChanges();
            }
            return RedirectToAction("BookPackage");
        }
    }
}

