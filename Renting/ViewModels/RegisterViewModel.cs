using System.ComponentModel.DataAnnotations;

namespace Renting.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Name is requied")]

        public string Name { get; set; }

        [Required(ErrorMessage = "Emial is requied")]
        [EmailAddress]

        public string Email { get; set; }

        [Required(ErrorMessage = "Password is requied")]
        [StringLength(40, MinimumLength = 8, ErrorMessage ="The {0} must be {2} and the max {1} charachters long")]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword", ErrorMessage = "Password des not match")]

        public string Password { get; set; }

        [Required(ErrorMessage = "Confirmation is requied")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]

        public string ConfirmPassword { get; set; }

    }
}
