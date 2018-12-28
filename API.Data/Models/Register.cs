using System.ComponentModel.DataAnnotations;

namespace API.Data.Models
{
    public class RegisterModel
    {
        [Required]
        public string UserName { get; set; }

        [EmailAddress]
        [Required]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A password is required")]
        [DataType(DataType.Password)]
        [MinLength(15, ErrorMessage = "Password must be at least 15 characters long for the best entropy.")]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        [DataType(DataType.Password)]
        [Display(Name= "Compare Password")]
        public string ComparePassword { get; set; }
    }

    public class ConfirmEmailModel
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
    }
}