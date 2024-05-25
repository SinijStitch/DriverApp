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

namespace SearchForDriversWebApp.Controllers
{
    [Authorize(Roles = "Admin, User")]
    public class ProfileController : Controller
    {
        private DriverDbContext _db;

        public ProfileController(DriverDbContext db)
        {
            this._db = db;
        }

        [HttpGet]
        public IActionResult Edit()
        {
            var user = _db.Users.FirstOrDefault(u => u.Email == @User.Identity.Name);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserViewModel userModel)
        {
            var user = _db.Users.FirstOrDefault(u => u.Email == @User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }
            else if (ModelState.IsValid)
            {
                user.Username = userModel.Username;
                user.Email = userModel.Email;
                user.Phone = userModel.Phone;
                _db.Users.Update(user);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Edit));
            }

            return View(user);
        }
    }
}