using System.ComponentModel.DataAnnotations;

namespace API.Authentication.Models
{
    public class RegisterModel
    {
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ComparePassword { get; set; }
    }
}