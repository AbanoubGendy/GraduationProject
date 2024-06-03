using System.ComponentModel.DataAnnotations;

namespace Graduation.PL.ViewModels
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage = "UserName Is Required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "FName Is Required")]
        public string Fname { get; set; }

        [Required(ErrorMessage = "LName Is Required")]
        public string Lname { get; set; }


        [Required(ErrorMessage = "Email Is Required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password Is Required")]
        [MinLength(5, ErrorMessage = "Minimum Length Password Is 5 ")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is Required")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Confirm Password not Match Password")]
        public string ConfirmPassword { get; set; }


        public bool IsAgree { get; set; }
    }
}
