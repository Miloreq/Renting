using System.ComponentModel.DataAnnotations;

namespace Renting.ViewModels
{
    public class VerifyEmailViewModel
    {
        [Required(ErrorMessage = "Emial is requied")]
        [EmailAddress]

        public string Email { get; set; }
    }
}
