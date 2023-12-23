
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
            var uId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var booked = from b in _db.Bookings
                        where b.UserId == uId
                        select b.Flight;
            var flight = from row in _db.Flights
                           select row;
            IEnumerable<Flight> flightList = flight.Except(booked).ToList();
            return View(flightList);
        }
        public async Task<IActionResult> bookNow(int id)
        {
            var uId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var exist = from b in _db.Bookings
                        where b.FlightId == id && b.UserId == uId
                        select b;
            if (exist.Any())
            {
                return RedirectToAction("Index");
            }
            else
            {
                ApplicationUser user = await _userManager.FindByIdAsync(uId);
                /* var flight = from f in _db.Flights
                                 where f.Id == flightId
                                 select f;*/
                var flight = _db.Flights.Find(id);
                var booking = new Booking
                {
                    UserId = uId,
                    User = user,
                    FlightId = id,
                    Flight = flight
                };
                _db.Bookings.Add(booking);
                _db.SaveChanges();
                return RedirectToAction("bookingList");
            }
        }
        public ActionResult bookingList()
        {
            
           var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var bookings = from row in _db.Bookings
                           where row.UserId == id
                           select row.Flight;
            List<Flight> bookingList = bookings.ToList();
            return View(bookingList);
        }
        public ActionResult cancelFlight(int id)
        {
            var uId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var booking = _db.Bookings.FirstOrDefault(b => b.UserId == uId  && b.FlightId == id);
            Console.WriteLine("********" + booking + "********");
            if (booking != null)
            {
                _db.Bookings.Remove(booking);
                _db.SaveChanges();
            }   
              return RedirectToAction("bookingList");
        }
    }
}
