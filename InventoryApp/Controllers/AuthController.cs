using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    public class AuthController : Controller
    {
        // GET: AuthController
        public ActionResult Login()
        {
            return View();
        }

    }
}
