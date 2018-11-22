using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace API.Authentication.Custom
{
    public class CustomPasswordValidator<TUser> : IPasswordValidator<TUser> where TUser : class
    {
        public async Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user, string password)
        {
            var username = await manager.GetUserNameAsync(user);

            if (username == password)
            {
                return IdentityResult.Failed(new IdentityError { Code = "PassInName", Description = "Cannot have the password in the user name"});
            }

            if (password.Contains("password"))
            {
                return IdentityResult.Failed(new IdentityError { Code = "PassInPass", Description = "Cannot have the word 'password' in the password"});
            }

            return IdentityResult.Success;
        }
    }
}