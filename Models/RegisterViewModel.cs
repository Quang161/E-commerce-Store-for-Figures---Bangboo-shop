using System.ComponentModel.DataAnnotations;

namespace Bangboo_E_Commerce.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "User Name")]
        public string IdPhaethon { get; set; } = null!;

        [Required]
        [Display(Name = "Full Name")]
        public string FullNamePhatheon { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        [Display(Name = "Password")]
        public string Password { get; set; } = null!;
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [Display(Name = "Email")]
        public string Email { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        public DateTime CreateAt { get; set; }
    }
}
