using System.ComponentModel.DataAnnotations;

namespace Renting.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Emial is requied")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is requied")]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "The {0} must be {2} and the max {1} charachters long")]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        [Compare("ConfirmNewPassword", ErrorMessage ="Password does not match")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirmation is requied")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        public string ConfirmNewPassword { get; set; }
    }
}
