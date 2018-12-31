using System;
using System.Threading.Tasks;
using API.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace API.Data.Services
{
    public interface IAccountRepisitory
    {
        Task<IdentityUser> GetUser(string userName);
        UserInfo GetUserInfo(IdentityUser iUser);
        void UpdateUserInfo(IdentityUser iUser, UserInfoForUpdateOrCreation userInfo);
    }
}