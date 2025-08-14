using System.Security.Cryptography;
using System.Text;
using InventoryApp.Data;
using InventoryApp.Models;
using InventoryApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AuthController
        public ActionResult Login()
        {
            ViewBag.Message = "";
            return View();

        }
        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            var hashedPassword = ComputeSha256Hash(password);
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.PasswordHash == hashedPassword);

            if (user != null)
            {
                ViewBag.Message = "Authorized";
                HttpContext.Session.SetString("Email", user.Email);
                return RedirectToAction("Dashboard", "Inventory");
            }
            ViewBag.Message = "Unauthorized. Please signup first if you dont have an account yet";
            return View();

        }

        [HttpGet]
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signup(SignupViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);

            }

            if (_context.Users.Any(u => u.Email == model.Email))
            {
                ModelState.AddModelError("Email", "Email already registered.");
                 return View(model);
            }
            var user = new User
            {
                Email = model.Email,
                PasswordHash = ComputeSha256Hash(model.Password),
                Name = model.Name,
            };
            _context.Users.Add(user);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "User created successfully! Please log in.";
            return RedirectToAction("Login");
        }

        private static string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                StringBuilder builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

    }
}
