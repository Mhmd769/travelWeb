// PackageController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using user_adminlogin.Data;
using user_adminlogin.Models;

namespace Flight_dbproject.Controllers
{
    [Authorize]
    public class PackageController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PackageController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var packages = _context.Packages.Include(p => p.Flight).ToList();
            return View(packages);
        }
    }
}

