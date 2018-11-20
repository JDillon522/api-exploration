using Microsoft.AspNetCore.Mvc;
using API.Authentication.Models;
using API.Authentication.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System;

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

        [HttpPost("register")]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterNewUser([FromBody] RegisterModel registerData)
        {
            if (ModelState.IsValid) {
                if (await _userManager.FindByNameAsync(registerData.UserName) != null) {
                    ModelState.AddModelError("Error", "User name already exists");
                    return BadRequest(ModelState);
                }

                UserModel newUser = new UserModel()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = registerData.UserName,
                };

                var result = await _userManager.CreateAsync(newUser, registerData.Password);

                if (result.Succeeded) {
                    return Ok();
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }

            return BadRequest(ModelState);
        }
    }
}