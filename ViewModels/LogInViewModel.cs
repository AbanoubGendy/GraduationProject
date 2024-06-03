using System.ComponentModel.DataAnnotations;

namespace Graduation.PL.ViewModels
{
    public class LogInViewModel
    {
        [Required(ErrorMessage = "Email Is Required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password Is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
