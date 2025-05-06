using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Entities;
using SerivicesContracts.Enums;

namespace SerivicesContracts.DTO
{
    public class PersonResponseDTO
    {
        public Guid PersonID { get; set; }
        public string? PersonName { get; set; }
        public string? Email { get; set; }
        public string? phoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public Guid? CountryID { get; set; }
        public string? Country { get; set; }
        public string? Address { get; set; }
        public double? Age { get; set; }

        public override bool Equals(object? obj)
        {
            if(obj == null)
            {
                return false;
            }
            if(obj.GetType() != typeof(PersonResponseDTO))
            {
                return false;
            }
            PersonResponseDTO person = (PersonResponseDTO)obj;

            return person.PersonID == person.PersonID && person.PersonName == person.PersonName;
        }
        public Person ToPerson()
        {
            return new Person
            {
                PersonID = PersonID,
                PersonName = PersonName,
                Email = Email,
                phoneNumber = phoneNumber ,
                DateOfBirth = DateOfBirth,
                Gender = Gender,
                CountryID = CountryID,
                Address = Address,
            };
        }

        public PersonUpdateDTO ToPersonUpdateRequest()
        {
            return new PersonUpdateDTO() { PersonID = PersonID, PersonName = PersonName, Email = Email, DateOfBirth = DateOfBirth, Gender = (GenderEnum)Enum.Parse(typeof(GenderEnum), Gender, true), Address = Address, CountryID = CountryID, phoneNumber = phoneNumber };
        }
    }
    public static class PersonExentions
    {
        public static PersonResponseDTO ToPersonResponseDTO(this Person person)
        {
            return new PersonResponseDTO
            {
                PersonID = person.PersonID,
                PersonName = person.PersonName,
                Email = person.Email,
                phoneNumber = person.phoneNumber,
                DateOfBirth = person.DateOfBirth,
                Gender = person.Gender,
                CountryID = person.CountryID,
                Address = person.Address,
                Country = person.Country?.CountryName,
                Age = (person.DateOfBirth != null) ? Math.Round((DateTime.Now - person.DateOfBirth.Value).TotalDays / 365.25) : null
            };
        }
    }

}
