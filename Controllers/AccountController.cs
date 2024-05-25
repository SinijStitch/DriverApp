using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using SearchForDriversWebApp.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using SearchForDriversWebApp.ViewModels;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace SearchForDriversWebApp.Controllers
{
    public class AccountController : Controller
    {
        private DriverDbContext db;
        public AccountController(DriverDbContext context)
        {
            db = context;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

                if (user != null && VerifyPassword(model.Password, user.Password))
                {
                    await Authenticate(model.Email);
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Некоректний логін або пароль");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

                if (user == null)
                {
                    // хешуємо пароль перед збереженням в базі даних
                    string hashedPassword = HashPassword(model.Password);

                    // додаємо користувача в бд
                    db.Users.Add(new User
                    {
                        Username = model.Username,
                        Phone = model.Phone,
                        Email = model.Email,
                        Password = hashedPassword,
                        Role = "User"
                    });
                    db.SaveChanges();

                    await Authenticate(model.Email);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Некоректний логін або пароль");
                }
            }

            return View(model);
        }

        // Додайте інші методи контролера

        // Метод для хешування паролю
        private string HashPassword(string password)
        {
            // Встановіть адекватні значення для параметрів алгоритму PBKDF2
            var salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            string hashed = Convert.ToBase64String(
                KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8
                )
            );

            return $"{Convert.ToBase64String(salt)}:{hashed}";
        }

        // Метод для перевірки паролю
        private bool VerifyPassword(string enteredPassword, string storedPassword)
        {
            var parts = storedPassword.Split(':');
            var salt = Convert.FromBase64String(parts[0]);
            var hashedEnteredPassword = Convert.ToBase64String(
                KeyDerivation.Pbkdf2(
                    password: enteredPassword,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8
                )
            );

            return parts[1] == hashedEnteredPassword;
        }

        private async Task Authenticate(string userName)
        {
            User user = await db.Users.FirstOrDefaultAsync(u => u.Email == userName);

            if (user == null)
            {
                return;
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName),
            };

            if (!string.IsNullOrEmpty(user.Role))
            {
                claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role));
            }

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}