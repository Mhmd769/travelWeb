using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace user_adminlogin.Controllers
{
    public class FlightHistoryController : Controller
    {
        [Authorize(Roles = "Admin")]

        public IActionResult Index()
        {
            return View();
        }
    }
}
