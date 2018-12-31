using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace API.Data.Models
{
    public class UserModel : IdentityUser
    {
        public string Locale { get; set; } = "en-US";
        public string OrgId { get; set; }
        public Guid UserInfoId { get; set; }
    }

    public class Organization
    {
        public string Id { get; set; }
        public string Name { get; set; }

    }

    public class UserInfo
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class UserInfoForUpdateOrCreation
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class UserViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FullName {
            get
            {
                return $"{this.FirstName} {this.LastName}";
            }
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string PhoneNumber { get; set; }
    }
}