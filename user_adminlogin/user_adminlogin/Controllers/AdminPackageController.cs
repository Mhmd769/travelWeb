using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using user_adminlogin.Data;
using user_adminlogin.Models;
using System.Linq;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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

        public IActionResult Index()
        {
            var packages = _context.Packages.ToList();
            return View(packages);
        }
        [HttpGet]
        public IActionResult AddPackage()
        {
            ViewData["Flights"] = new SelectList(_context.Flights.Select(f => new { Id = f.Id, Name = f.flight_Name}).ToList(), "Id", "Name");
            return View();
        }
        [HttpPost]
        public IActionResult AddPackage(Package package)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Packages.Add(package);
                    _context.SaveChanges();

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
            ViewData["FlightIds"] = new SelectList(_context.Flights.Select(f => f.Id).ToList()); // Retrieve flight ids again for the dropdown
            return View(package);
        }


        [HttpGet]
        public IActionResult UpdatePackage(int id)
        {
            var package = _context.Packages.Find(id);
            if (package == null)
            {
                return NotFound();
            }

            return View(package);
        }

        [HttpPost]
        public IActionResult UpdatePackage(Package package)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Packages.Update(package);
                    _context.SaveChanges();

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
            return View(package);
        }

        [HttpGet]
        public IActionResult DeletePackage(int id)
        {
            var package = _context.Packages.Find(id);
            if (package == null)
            {
                return NotFound();
            }

            return View(package);
        }

        [HttpPost, ActionName("DeletePackage")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePackageConfirmed(Package package)
        {
            var packagetoDelete = _context.Packages.Find(package.Id);
            if (packagetoDelete == null)
            {
                return NotFound();
            }

            _context.Packages.Remove(packagetoDelete);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }


    }


}
