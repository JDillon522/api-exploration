using Microsoft.AspNetCore.Mvc;
using API.Authentication.Models;
using API.Authentication.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System;

namespace API.Authentication.Controllers
{
    [Route("/api")]
    public class LoginController : Controller
    {
        private readonly UserManager<UserModel> _userManager;

        public LoginController(UserManager<UserModel> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("/login/register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterNewUser(RegisterModel registerData)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }


            if (await _userManager.FindByNameAsync(registerData.UserName) != null) {
                return BadRequest();
            }

            UserModel newUser = new UserModel()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = registerData.UserName,
            };

            var result = await _userManager.CreateAsync(newUser, registerData.Password);

            return Ok();
        }
    }
}