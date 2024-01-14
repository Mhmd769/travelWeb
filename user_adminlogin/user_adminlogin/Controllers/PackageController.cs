using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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

            var packages = _db.Packages.FromSqlInterpolated($"EXEC GetAvailablePackages {uId}").ToList();

            return View(packages);
        }

        public async Task<IActionResult> bookNow(int id)
        {
            var uId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var userIdParam = new SqlParameter("@UserId", SqlDbType.NVarChar) { Value = uId };
            var packageIdParam = new SqlParameter("@PackageId", SqlDbType.Int) { Value = id };

            await _db.Database.ExecuteSqlInterpolatedAsync($"EXEC BookPackage {userIdParam}, {packageIdParam}");

            return RedirectToAction("BookPackage");
        }

        public ActionResult BookPackage()
        {   
            var uId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var userIdParam = new SqlParameter("@UserId", SqlDbType.NVarChar) { Value = uId };

            var packages = _db.Packages.FromSqlInterpolated($"EXEC GetUserBookedPackages {userIdParam}").ToList();

            return View(packages);
        }

        public ActionResult cancelPackage(int id)
        {
            var uId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var userIdParam = new SqlParameter("@UserId", SqlDbType.NVarChar) { Value = uId };
            var packageIdParam = new SqlParameter("@PackageId", SqlDbType.Int) { Value = id };

            _db.Database.ExecuteSqlInterpolated($"EXEC CancelUserPackage {userIdParam}, {packageIdParam}");

            return RedirectToAction("BookPackage");
        }
    }
}
