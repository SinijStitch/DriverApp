using Microsoft.AspNetCore.Mvc;
using SearchForDriversWebApp.Models;
using System.Diagnostics;
using SearchForDriversWebApp.ViewModels;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using SearchForDriversWebApp.Services;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using System.Net.Mail;
using System.Net;

namespace SearchForDriversWebApp.Controllers
{
    [Authorize(Roles = "Admin, User, Driver")]

    public class HomeController : Controller
    {
        private DriverDbContext db;
        private readonly ILogger<HomeController> logger;
        private readonly Services.EmailService emailService;

        public HomeController(DriverDbContext db, ILogger<HomeController> logger, EmailService emailService)
        {
            this.db = db;
            this.logger = logger;
            this.emailService = emailService;
        }

        public async Task<IActionResult> Index()
        {          

            return View();
        }


        [HttpPost]
        public IActionResult Index(TripModel model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.FirstOrDefault(x => x.Email == User.Identity.Name);
                var trip = new Trip
                {
                    DepartureLocation = model.DepartureLocation,
                    ArrivalLocation = model.ArrivalLocation,
                    Distance = model.Distance,
                    DateOfTrip = model.DateOfTrip,
                    Status = "Опрацьовується оператором",
                    UserId = user.Id
                };
                db.Trips.Add(trip);
                db.SaveChanges();
                emailService.SendEmailDefault(user.Email, $"Шановний(на) {user.Username}, дякуємо за використання сервісу Yavir!<br/>Ваша поїздка {trip.DateOfTrip} {trip.DepartureLocation} - {trip.ArrivalLocation} з відстанню {trip.Distance}км успішно створена!<br/>Наразі статус Вашого замовлення:{trip.Status}.<br/>Очікуйте оновлень статусу замовлення!");

                return RedirectToAction("Index");
            }

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult AssignmentTrips()
        {
            IEnumerable<Assignment> assignments = db.Assignments.Include(x=>x.User).Include(x=>x.Trip).Where(x=>x.User.Email == User.Identity.Name && x.User.Role == "Driver");
            Dictionary<int, string> usernamesDictionary = db.Users.ToDictionary(user => user.Id, user => user.Username);

            ViewBag.Usernames = usernamesDictionary;
            return View(assignments);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatus(int tripId)
        {
            var trip = await db.Trips.FindAsync(tripId);

            if (trip != null)
            {
                trip.Status = "Водій очікує на вас";
                db.Trips.Update(trip);
                await db.SaveChangesAsync();
            }

            return RedirectToAction(nameof(AssignmentTrips));
        }

        public IActionResult UserTrips()
        {
                var trips = db.Trips.Include(x => x.User).Where(x => x.User.Email == User.Identity.Name && x.User.Role == "User").ToList();
                return View(trips);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult NotFoundPage()
        {
            Response.StatusCode = 404;
            return View("NotFound");
        }
    }
}