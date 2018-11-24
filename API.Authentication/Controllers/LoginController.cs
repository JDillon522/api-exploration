using Microsoft.AspNetCore.Mvc;

namespace API.Authentication.Controllers
{
    [Route("login")]
    public class LoginController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }
    }
}