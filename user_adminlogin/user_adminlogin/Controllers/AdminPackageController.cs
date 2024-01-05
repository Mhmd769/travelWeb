using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using user_adminlogin.Data;
using user_adminlogin.Models;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace user_adminlogin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminPackageController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminPackageController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var packages = await _context.Packages.FromSqlRaw("EXEC GetPackages").ToListAsync();
            return View(packages);
        }

        [HttpGet]
        public IActionResult AddPackage()
        {
            ViewData["Flights"] = new SelectList(_context.Flights.Select(f => new { Id = f.Id, Name = f.flight_Name }).ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult AddPackage(Package package)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Database.ExecuteSqlInterpolated($"EXEC AddPackage {package.Package_details}, {package.Price}, {package.FlightId}");

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Log the exception or handle it appropriately
                    // For now, let's return an error view with the exception details
                    return RedirectToAction("Index");
                }
            }

            // If ModelState is not valid, return back to the form with validation errors
            ViewData["Flights"] = new SelectList(_context.Flights.Select(f => new { Id = f.Id, Name = f.flight_Name }).ToList(), "Id", "Name");
            return View(package);
        }

        [HttpGet]
        public IActionResult UpdatePackage(int id)
        {
            // Use AsEnumerable to execute the raw SQL query and materialize the results
            var package = _context.Packages.FromSqlRaw($"EXEC GetPackageById {id}").AsEnumerable().FirstOrDefault();

            if (package == null)
            {
                return NotFound();
            }

            ViewData["Flights"] = new SelectList(_context.Flights.Select(f => new { Id = f.Id, Name = f.flight_Name }).ToList(), "Id", "Name");
            return View(package);
        }

        [HttpPost]
        public IActionResult UpdatePackage(Package package)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Database.ExecuteSqlInterpolated($"EXEC UpdatePackage {package.Id}, {package.Package_details}, {package.Price}, {package.FlightId}");

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Log the exception or handle it appropriately
                    // For now, let's return an error view with the exception details
                    return RedirectToAction("Index");
                }
            }

            // If ModelState is not valid, return back to the form with validation errors
            ViewData["Flights"] = new SelectList(_context.Flights.Select(f => new { Id = f.Id, Name = f.flight_Name }).ToList(), "Id", "Name");
            return View(package);
        }


        [HttpGet]
        public IActionResult DeletePackage(int id)
        {
            // Materialize the results using AsEnumerable()
            var package = _context.Packages.FromSqlRaw($"EXEC GetPackageById {id}").AsEnumerable().FirstOrDefault();

            if (package == null)
            {
                return NotFound();
            }

            return View(package);
        }

        [HttpPost, ActionName("DeletePackage")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePackageConfirmed(int id)
        {
            try
            {
                // No need to fetch the package before deleting
                _context.Database.ExecuteSqlInterpolated($"EXEC DeletePackage {id}");

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                // For now, let's return an error view with the exception details
                return RedirectToAction("Index");
            }
        }

    }
}
