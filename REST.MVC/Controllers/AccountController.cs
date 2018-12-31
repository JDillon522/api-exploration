using System.Threading.Tasks;
using API.Data.Models;
using API.Data.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace REST.MVC.Controllers
{
    public class AccountController : Controller
    {
        private static AccountRepisitory _accountRepisitory;

        public AccountController(AccountRepisitory AccountRepisitory)
        {
            _accountRepisitory = AccountRepisitory;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IdentityUser iUser = await _accountRepisitory.GetUser(HttpContext.User.Identity.Name);
            UserInfo userInfo = _accountRepisitory.GetUserInfo(iUser);

            UserViewModel user = new UserViewModel(){
              Email = iUser.Email,
              UserName = iUser.UserName,
              FirstName = userInfo.FirstName,
              LastName = userInfo.LastName,
              Age = userInfo.Age,
              PhoneNumber = userInfo.PhoneNumber
            };

            ViewBag.User = user;
            return View();
        }

        [HttpPost] // Should be PUT but plain HTML is stupid and vunerable to DELETE and PUT apparently.
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAccount([FromForm] UserInfoForUpdateOrCreation UserInfoUpdate)
        {
            IdentityUser iUser = await _accountRepisitory.GetUser(HttpContext.User.Identity.Name);

            _accountRepisitory.UpdateUserInfo(iUser, UserInfoUpdate);

            return RedirectToAction("");
        }
    }
}