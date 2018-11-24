using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace API.Authentication.Controllers
{
    [Route("login")]
    public class LoginController : Controller
    {

        private IHostingEnvironment _hostingEnvironment { get; }

        public LoginController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }
    }
}