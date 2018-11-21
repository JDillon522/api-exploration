using System.ComponentModel.DataAnnotations;

namespace API.Data.Models
{
    public class RegisterModel
    {
        public string UserName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ComparePassword { get; set; }
    }

    public class ConfirmEmailModel
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
    }
}