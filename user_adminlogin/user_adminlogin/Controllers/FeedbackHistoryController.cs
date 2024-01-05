using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;  // Make sure to include this namespace
using user_adminlogin.Data;
using user_adminlogin.Models;

namespace user_adminlogin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FeedbackHistoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public FeedbackHistoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
{
        var feedbackList = _db.Feedback
            .Include(f => f.User)
            .Include(f => f.Flight)
            .ToList();

        return View(feedbackList);
    }


    }
}
