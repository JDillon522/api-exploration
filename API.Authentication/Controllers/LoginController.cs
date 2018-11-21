using Microsoft.AspNetCore.Mvc;
using API.Authentication.Models;
using API.Authentication.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace API.Authentication.Controllers
{
    [Route("api/login")]
    public class LoginController : Controller
    {
        private readonly UserManager<UserModel> _userManager;

        public LoginController(UserManager<UserModel> userManager)
        {
            _userManager = userManager;
        }


        [HttpPost("")]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (ModelState.IsValid)
            {
                UserModel user = await _userManager.FindByNameAsync(model.UserName);

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

                UserModel newUser = new UserModel()
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