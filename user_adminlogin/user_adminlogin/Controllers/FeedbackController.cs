using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using user_adminlogin.Data;
using user_adminlogin.Models;

namespace Flight_dbproject.Controllers
{
    [Authorize]

    public class FeedbackController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;

        public FeedbackController(UserManager<ApplicationUser> userManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }
        public IActionResult Index()
        {
            var users = from feedback in _db.Feedback
                        select feedback.User;
            foreach(var user in users)
            {
                Console.WriteLine("******************************" + user.name + "*************************");
            }
            
            var feedbacks = from feedback in _db.Feedback
                            select feedback;
                        IEnumerable<Feedback> feedbackList = feedbacks.ToList();
                        return View(feedbackList);
        }
        public IActionResult Add()
        {
            List<Flight> flights = _db.Flights.ToList(); 
            ViewBag.FlightList = new SelectList(flights, "Id", "flight_Name");
            return View(new Feedback());
        }
        [HttpPost]
        public async Task<IActionResult> Add(Feedback feedback)
        {
            if (feedback != null)
            {
                feedback.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;  
                feedback.User = await _userManager.FindByIdAsync(feedback.UserId);
                _db.Feedback.Add(feedback);
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public IActionResult myFeedbacks()
        {
            var users = from feedback in _db.Feedback
                        select feedback.User;
            foreach (var user in users)
            {
                Console.WriteLine("******************************" + user.name + "*************************");
            }
            var uId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var feedbacks = from feedback in _db.Feedback
                            where feedback.UserId == uId
                            select feedback;
            IEnumerable<Feedback> feedbackList = feedbacks.ToList();
            return View(feedbackList);
        }
        public IActionResult editFeedback(int Id)
        {
            List<Flight> flights = _db.Flights.ToList();
            ViewBag.FlightList = new SelectList(flights, "Id", "flight_Name");
            var feedback = from f in _db.Feedback
                           where f.Id == Id
                           select f;
            var singleFeedback = feedback.SingleOrDefault();
            return View(singleFeedback);
        }
        [HttpPost]
        public IActionResult editFeedback(Feedback feedback)
        {
            feedback.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _db.Feedback.Update(feedback);
            _db.SaveChanges();
            return RedirectToAction("myFeedbacks");
        }
        public IActionResult deleteFeedback(int Id)
        {
            var feedback = from f in _db.Feedback
                           where f.Id == Id
                           select f;
            var singleFeedback = feedback.SingleOrDefault();
            _db.Feedback.Remove(singleFeedback);
            _db.SaveChanges();
            return RedirectToAction("myFeedbacks");
        }
    }
}
