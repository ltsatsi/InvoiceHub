using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS_DomainLayer.Dtos.Account
{
    public class SignUpDto
    {
        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "Full Name is required")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Full Name must be between 3 and 20 characters")]
        public string FullName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Address Line is required")]
        [StringLength(52, MinimumLength = 3, ErrorMessage = "Address Line can only be between 3 and 20 characters")]
        public string AddressLine { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Compare(nameof(Password))]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
