using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using System.Xml.Linq;
using SerivicesContracts.Enums;

namespace SerivicesContracts.DTO
{
    public class PersonUpdateDTO
    
    {
        [Required(ErrorMessage = "Person ID can't be blank")]
        public Guid PersonID { get; set; }

        [Required(ErrorMessage = "Person Name can't be blank")]
        public string? PersonName { get; set; }

        [Required(ErrorMessage = "Email can't be blank")]
        [EmailAddress(ErrorMessage = "Email value should be a valid email")]
        public string? Email { get; set; }
        [RegularExpression("^[0-9]*$", ErrorMessage = "Phone number should contain 11 numbers only")]
        [StringLength(11)]
        [MinLength(11)]
        public string? phoneNumber { get; set; }

        public DateTime? DateOfBirth { get; set; }
        public GenderEnum? Gender { get; set; }
        public Guid? CountryID { get; set; }
        public string? Address { get; set; }

        /// <summary>
        /// Converts the current object of PersonAddRequest into a new object of Person type
        /// </summary>
        /// <returns>Returns Person object</returns>
        public Person ToPerson()
        {
            return new Person() { PersonID = PersonID, PersonName = PersonName, Email = Email,phoneNumber =phoneNumber, DateOfBirth = DateOfBirth, Gender = Gender.ToString(), Address = Address, CountryID = CountryID};
        }
    }
    
}
