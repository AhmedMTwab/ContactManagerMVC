using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace ContactManager.CORE.DTO
{
    public class UserRegisterDTO
    {
        [Required(ErrorMessage ="UserName is Required")]
        [Remote(action: "UserNameIsExist", controller: "UserRemoteValidations", ErrorMessage = "This UserName already Exists")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress]
        [Remote(action: "EmailIsExist", controller: "UserRemoteValidations", ErrorMessage = "This Email already Exists")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "ConfirmPassword is Required")]
        [Compare("Password",ErrorMessage ="Dosent Match The Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "PhoneNumber is Required")]
        [RegularExpression(@"^\+[0-9]{12}$", ErrorMessage = "Phone number must start with '+' followed by 12 digits (e.g., +201234567890)")]
        [StringLength(13, ErrorMessage = "Phone number should be 13 numbers")]
        [MinLength(13, ErrorMessage = "Phone number should be 13 numbers")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "PersonName is Required")]
        public string PersonName { get; set; }
        [Required(ErrorMessage = "DateOfBirth is Required")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
    }
}
