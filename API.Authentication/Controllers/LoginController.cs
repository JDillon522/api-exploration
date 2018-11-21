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
        private readonly UserManager<UserModel> _userManager;
        private readonly IAntiforgery _antiForgery;
        private readonly IUserClaimsPrincipalFactory<UserModel> _claimsFactory;
        private readonly SignInManager<UserModel> _signInManager;

        public LoginController(
            UserManager<UserModel> userManager,
            IAntiforgery antiforgery,
            IUserClaimsPrincipalFactory<UserModel> claimsPrincipalFactory,
            SignInManager<UserModel> signInManager
        )
        {
            _userManager = userManager;
            _antiForgery = antiforgery;
            _claimsFactory = claimsPrincipalFactory;
            _signInManager = signInManager;
        }


        [HttpPost("")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (ModelState.IsValid)
            {
                // NOTE: _userManager is more configureable but not as "out of the box"
                UserModel user = await _userManager.FindByNameAsync(model.UserName);
                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    if (!await _userManager.IsEmailConfirmedAsync(user))
                    {
                        ModelState.AddModelError("EmailConfirm", "Please confirm your email");
                        return BadRequest(ModelState);
                    }

                    var principal = await _claimsFactory.CreateAsync(user);
                    await HttpContext.SignInAsync("Identity.Application", principal);
                    return Ok();
                }

                // NOTE: _signInManager is straight forward and easy but may have some issues dpeneing on less conventional use cases
                // var signInResult = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);

                // if (signInResult.Succeeded) {
                //     return Ok();
                // }

                // if (signInResult.RequiresTwoFactor) {
                //     //
                // }

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
        [Authorize]
        public async Task<IActionResult> RegisterNewUser([FromBody] RegisterModel model)
        {
            return await HandleSignup(model);
        }

        [HttpPost("signup")]
        [AllowAnonymous]
        public async Task<IActionResult> SelfRegister([FromBody] RegisterModel model)
        {
            return await HandleSignup(model);
        }

        private async Task<IActionResult> HandleSignup(RegisterModel model)
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
                    Email = model.Email
                };

                IdentityResult result = await _userManager.CreateAsync(newUser, model.Password);

                if (result.Succeeded) {
                    string token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                    string confirmEmailUrl = Url.Action("ConfirmEmail", "Login", new { token = token, email = newUser.Email }, Request.Scheme);
                    System.IO.File.WriteAllText("testOutput.txt", $"Confirm Link ------ ${confirmEmailUrl}");

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

        [HttpPost("confirm")]
        public async Task<IActionResult> SendConfirmEmailAddress([FromBody] ConfirmEmailModel model)
        {
            if (ModelState.IsValid) {
                UserModel user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    string confirmEmailUrl = Url.Action("ConfirmEmailAddress", "Login", new { token = token, email = user.Email }, Request.Scheme);
                    System.IO.File.WriteAllText("testOutput.txt", $"Confirm Link ------ ${confirmEmailUrl}");

                    return Ok();
                }

                ModelState.AddModelError("NotFound", $"Account with email {model.Email} was not found");
                return BadRequest(ModelState);
            }

            return BadRequest(ModelState);
        }


        [HttpGet("confirm")]
        public async Task<IActionResult> ConfirmEmailAddress(string token, string email)
        {
            UserModel user = await _userManager.FindByEmailAsync(email);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (user != null)
            {
                IdentityResult result = await _userManager.ConfirmEmailAsync(user, token);

                if (!result.Succeeded)
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return BadRequest(ModelState);
                }

                return Ok();
            }

            ModelState.AddModelError("UserNotFound", $"User with email {email} was not found");
            return BadRequest(ModelState);
        }

        [HttpPost("forgot")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                UserModel user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    string token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var resetUrl = Url.Action(
                        "ResetPassword",
                        "Login",
                        new {
                            token = token,
                            email = user.Email
                        },
                        Request.Scheme
                    );

                    // DEMO Stuff
                    System.IO.File.WriteAllText("resetLink.txt", $"Reset Password Link ------- {resetUrl}");
                }
                else
                {
                    // Send an email to the user
                }

                return Ok();
            }

            return BadRequest(ModelState);
        }

        [HttpGet("reset")]
        public async Task<IActionResult> ResetPassword(string token, string email)
        {
            return RedirectToAction("ResetPassword", new ResetPasswordModel() { Token = token, Email = email });
        }

        [HttpPost("reset", Name = "ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                UserModel user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

                    if (!result.Succeeded)
                    {
                        foreach (IdentityError error in result.Errors)
                        {
                            ModelState.AddModelError(error.Code, error.Description);
                        }

                        return BadRequest(ModelState);
                    }

                    return Ok();
                }
                else
                {
                    // Send an email to the user
                }

                return Ok();
            }

            return BadRequest(ModelState);
        }
    }
}