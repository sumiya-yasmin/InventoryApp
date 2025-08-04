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
                Message = "Authorized";
            }
            ViewBag.Message = Message;
            return View();

        }

        public ActionResult Signup()
        {
            return View();
        }

    }
}
