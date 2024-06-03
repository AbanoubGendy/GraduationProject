using System.ComponentModel.DataAnnotations;

namespace Graduation.PL.ViewModels
{
    public class ResetpasswordViewModel
    {
        [Required(ErrorMessage = "New Password Is Required")]
        [MinLength(5, ErrorMessage = "Minimum Length Password Is 5 ")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm Password is Required")]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "Confirm Password not Match Password")]
        public string ConfirmPassword { get; set; }
    }
}
