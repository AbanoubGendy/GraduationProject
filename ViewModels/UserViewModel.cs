using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System;

namespace Graduation.PL.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(50, ErrorMessage = "Max length of name is 50 Char")]
        [MinLength(5, ErrorMessage = "Min length of name is 5 Char")]
        public string Name { get; set; }

        [Range(22, 30)]
        public int Age { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$",
            ErrorMessage = "Address Must be Like 123-Street-City-Country")]
        public string Address { get; set; }

        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Phone Number")]
        [Phone]
        public string PhoneNumber { get; set; }

        [Display(Name = "Hiring Date")]
        public DateTime HiringDate { get; set; } // Date For HiringDate 

        public IFormFile Image { get; set; }

        public string ImageName { get; set; }

    }
}
