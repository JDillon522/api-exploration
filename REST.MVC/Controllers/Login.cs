using Microsoft.AspNetCore.Mvc;

namespace REST.MVC.Controllers
{
    public class Login : Controller
    {
         public IActionResult Index()
        {
            return View();
        }
    }
}