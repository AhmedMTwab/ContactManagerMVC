using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using SerivicesContracts.Enums;

namespace SerivicesContracts.DTO
{
    public class PersonAddDTO
    {
        [Required(ErrorMessage ="PersonName is required")]
        public string PersonName { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        [RegularExpression("^[0-9]*$", ErrorMessage = "Phone number should contain 11 numbers only")]
        [StringLength(11)]
        [MinLength(11)]
        public string? phoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public GenderEnum? Gender { get; set; }
        public Guid? CountryID { get; set; }
        public string? Address { get; set; }
    }
    public static class PersonAddDTOExtension
    {
        public static Person ToPerson(this PersonAddDTO personAddDTO)
        {
            return new Person
            {
                PersonName = personAddDTO.PersonName,
                Email = personAddDTO.Email,
                phoneNumber = personAddDTO.phoneNumber,
                DateOfBirth = personAddDTO.DateOfBirth,
                Gender = personAddDTO.Gender.ToString(),
                CountryID = personAddDTO.CountryID,
                Address = personAddDTO.Address,
               
            };
        }
    }
}
