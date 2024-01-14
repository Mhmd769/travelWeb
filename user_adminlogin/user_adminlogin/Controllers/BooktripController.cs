
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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
            try
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
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlException && sqlException.Number == 51000)
                {
                    // Display a user-friendly error message for double booking
                    ModelState.AddModelError("", "User cannot book two flights on the same date.");

                    // Add this line to include JavaScript in your view
                    TempData["ShowAlert"] = true;
                }
                else
                {
                    // Handle other database update exceptions as needed
                    // Log the exception for further investigation
                    // ...

                    TempData["ShowAlert"] = false;

                    // If it's not a double booking exception, don't set ViewBag.ShowAlert to true
                }

                return RedirectToAction("Index");
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
