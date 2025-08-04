using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    public class AuthController : Controller
    {
        // GET: AuthController
        public ActionResult Login()
        {
             ViewBag.Message = "";
            return View();

        }
        [HttpPost]
        public ActionResult Login(string emailtxt, string passwordtxt)
        {
            string Message = "Unauthorized";
            
            if (emailtxt == "simi@nasif" && passwordtxt == "nasif")
            {
                ViewBag.Message = "Authorized";
                HttpContext.Session.SetString("Email", "emailtxt");
               return RedirectToAction("Dashboard", "Inventory");
            }
            ViewBag.Message = Message;
            return View();

        }

        [HttpGet]
        public ActionResult Logout() {
            HttpContext.Session.Clear();
            return View("Login");
        }

        public ActionResult Signup()
        {
            return View();
        }

    }
}
