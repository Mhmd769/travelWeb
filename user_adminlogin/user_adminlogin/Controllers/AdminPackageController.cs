using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace user_adminlogin.Controllers
{
    public class AdminPackageController : Controller
    {
        [Authorize(Roles = "Admin")]

        public IActionResult Index()
        {
            return View();
        }
    }
}
