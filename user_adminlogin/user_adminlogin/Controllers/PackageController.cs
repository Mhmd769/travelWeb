using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flight_dbproject.Controllers
{
    [Authorize]

    public class PackageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
