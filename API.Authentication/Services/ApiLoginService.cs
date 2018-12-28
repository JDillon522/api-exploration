using System;
using System.Threading.Tasks;
using API.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Authentication.Services
{
    public class ApiLoginService
    {
        private readonly UserManager<UserModel> _userManager;

        private readonly IUserClaimsPrincipalFactory<UserModel> _claimsFactory;
        private readonly SignInManager<UserModel> _signInManager;

        public ApiLoginService(
            UserManager<UserModel> userManager,
            IUserClaimsPrincipalFactory<UserModel> claimsPrincipalFactory,
            SignInManager<UserModel> signInManager
        )
        {
            _userManager = userManager;
            _claimsFactory = claimsPrincipalFactory;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> AddValidatedNewUser(RegisterModel model)
        {
            UserModel newUser = new UserModel()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                Email = model.Email
            };

            IdentityResult result = await _userManager.CreateAsync(newUser, model.Password);

            return result;
        }

    }
}