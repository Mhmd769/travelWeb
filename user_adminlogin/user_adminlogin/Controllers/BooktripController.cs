
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Flight_dbproject.Controllers
{
    [Authorize]

    public class BooktripController : Controller
    {
        public BooktripController()
        {
          
        }

        public IActionResult Index()
        {
            //// Retrieve unique destinations from the database
            //var destinations = _dbContext.Flights.Distinct().ToList();

            //// Pass the destinations to the view
            //ViewBag.Destinations = destinations;

            return View();
        }

    }
}
