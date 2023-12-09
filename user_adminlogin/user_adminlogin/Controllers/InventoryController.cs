using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace user_adminlogin.Controllers
{
    [Authorize(Roles ="Admin")]
    public class InventoryController : Controller
    {
        public IActionResult GetAll()
        {
            // Your action method logic here
            return View();
        }
    }

}