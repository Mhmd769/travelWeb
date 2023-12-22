
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using user_adminlogin.Data;
using user_adminlogin.Models;

namespace Flight_dbproject.Controllers
{
    [Authorize]
    public class BooktripController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;

        public BooktripController(UserManager<ApplicationUser> userManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }
        public IActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Flight flight)
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            /* ApplicationUser user = from _userManager
                         where Id = id
                         select * ;*/
            var booking = new Booking
            {
                UserId = id,
                User = user,
                FlightId = flight.Id,
                Flight = flight,
            };
            _db.Bookings.Add(booking);
            _db.Flights.Add(flight);
            _db.SaveChanges();
            return View();
        }

        public ActionResult bookingList()
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var bookings = from row in _db.Bookings
                           where row.UserId == id
                           select row;
            List<Booking> bookingList = bookings.ToList();
            return View(bookingList);
        }
    }
}
