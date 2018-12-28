using System.Net.Http;
using System.Threading.Tasks;
using API.Authentication.Services;
using API.Data.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace REST.MVC.Controllers
{
    public class Login : Controller
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly ApiLoginService _loginService;
        private readonly IUserClaimsPrincipalFactory<UserModel> _claimsFactory;

        public Login(
            UserManager<UserModel> userManager,
            ApiLoginService loginService,
            IUserClaimsPrincipalFactory<UserModel> claimsFactory
        )
        {
            _userManager = userManager;
            _loginService = loginService;
            _claimsFactory = claimsFactory;
        }

        public IActionResult Index([FromQuery] string userName, ModelStateDictionary model)
        {
            ViewData["userName"] = userName;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Index(LoginModel model)
        {
            UserModel user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                // if (!await _userManager.IsEmailConfirmedAsync(user))
                // {
                //     ModelState.AddModelError("EmailConfirm", "Please confirm your email");
                // }
                // else
                // {
                    var principal = await _claimsFactory.CreateAsync(user);
                    await HttpContext.SignInAsync("Identity.Application", principal);
                    return RedirectToAction("Index", "Home");
                // }
            }
            else
            {
                ModelState.AddModelError("", "UserName not found or password was incorrect");
            }

            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid) {
                if (await _userManager.FindByNameAsync(model.UserName) != null) {
                    ModelState.AddModelError("Error", "User name already exists");

                } else {
                    IdentityResult result = await _loginService.AddValidatedNewUser(model);

                    if (result.Succeeded) {
                        return RedirectToAction("Index", "Home");
                    }

                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Identity.Application");
            return RedirectToAction("Index");
        }
    }
}