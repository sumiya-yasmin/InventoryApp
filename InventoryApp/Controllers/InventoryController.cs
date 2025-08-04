using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    public class InventoryController : Controller
    {
        // GET: InventoryController
        public ActionResult Dashboard()
        {
            return View();
        }

    }
}
