using Microsoft.AspNetCore.Mvc;
using API.Data.Models;
using API.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;

namespace API.Authentication.Controllers
{
    [Route("api/login")]
    public class LoginController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IAntiforgery _antiForgery;

        public LoginController(UserManager<IdentityUser> userManager, IAntiforgery antiforgery)
        {
            _userManager = userManager;
            _antiForgery = antiforgery;
        }


        [HttpPost("")]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = await _userManager.FindByNameAsync(model.UserName);

                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    ClaimsIdentity identity = new ClaimsIdentity("cookies");
                    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
                    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.UserName));

                    await HttpContext.SignInAsync("cookies", new ClaimsPrincipal(identity));

                    return Ok();
                }
            }

            ModelState.AddModelError("LoginError", "Invalid UserName or Password");
            return BadRequest(ModelState);
        }

        [HttpGet("check")]
        [Authorize]
        public IActionResult CheckLogin()
        {
            return Ok();
        }

        [HttpPost("register")]
        // [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> RegisterNewUser([FromBody] RegisterModel model)
        {
            if (ModelState.IsValid) {
                if (await _userManager.FindByNameAsync(model.UserName) != null) {
                    ModelState.AddModelError("Error", "User name already exists");
                    return BadRequest(ModelState);
                }

                IdentityUser newUser = new IdentityUser()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = model.UserName,
                };

                IdentityResult result = await _userManager.CreateAsync(newUser, model.Password);

                if (result.Succeeded) {
                    return Ok();
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }

            return BadRequest(ModelState);
        }

        [HttpPost("signup")]
        // [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> SelfRegister([FromBody] RegisterModel model)
        {
            if (ModelState.IsValid) {
                if (await _userManager.FindByNameAsync(model.UserName) != null) {
                    ModelState.AddModelError("Error", "User name already exists");
                    return BadRequest(ModelState);
                }

                IdentityUser newUser = new IdentityUser()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = model.UserName,
                };

                IdentityResult result = await _userManager.CreateAsync(newUser, model.Password);

                if (result.Succeeded) {
                    return Ok();
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }

            return BadRequest(ModelState);
        }

    }
}