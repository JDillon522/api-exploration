using Microsoft.AspNetCore.Mvc;

namespace REST.MVC.Controllers
{
    public class Login : Controller
    {
         public IActionResult Index([FromQuery] string userName)
        {
            ViewData["userName"] = userName;
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
    }
}