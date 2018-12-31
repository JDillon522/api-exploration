using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data.Entities;
using API.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using System.Collections.Generic;

namespace API.Data.Services
{
    public class AccountRepisitory : IAccountRepisitory
    {
        private readonly UserManager<UserModel> _userManager;
        private UserDbContext _context;
        public AccountRepisitory(
            UserManager<UserModel> userManager,
            UserDbContext context
        )
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IdentityUser> GetUser(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        public UserInfo GetUserInfo(IdentityUser iUser)
        {
            UserInfo userInfo = _context.UserInfo.FirstOrDefault(i => i.UserId == Guid.Parse(iUser.Id));

            if (userInfo == null) {
                userInfo = new UserInfo();
            }
            return userInfo;
        }

        public void UpdateUserInfo(IdentityUser iUser, UserInfoForUpdateOrCreation userInfo)
        {
            UserInfo updatedInfo = Mapper.Map<UserInfo>(userInfo);
            updatedInfo.UserId = Guid.Parse(iUser.Id);

            _context.UserInfo.Update(updatedInfo);
            _context.SaveChanges();

            return;
        }
    }
}