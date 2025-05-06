using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactManager.CORE.Domain.Entities.IdentityEntites;
using ContactManager.CORE.Helpers;
using ContactManager.CORE.ServiciesContracts.IIdentityServices;
using ContactManager.CORE.ServiciesContracts.IPersonServices;
using CustomExceptions;
using Entities;
using Microsoft.AspNetCore.Identity;
using RepositoryContract;
using SerivicesContracts.DTO;

namespace ContactManager.CORE.Servicies.PersonServices
{
    public class PersonGetter:IPersonGetter
    {
        private readonly IPersonRepository _personRepository;
        private readonly ICurrentApplicationUserService _currentApplicationUserService;


        //constructor
        public PersonGetter(IPersonRepository personRepository,ICurrentApplicationUserService currentApplicationUserService)
        {
            _personRepository = personRepository;
            this._currentApplicationUserService = currentApplicationUserService;
        }
        public async Task<List<PersonResponseDTO>> GetAllPersons()
        {

            List<Person> persons = await _personRepository.GetAllPersons();
            return persons.Where(p => p.ApplicationUserID == _currentApplicationUserService.currentApplicationUserId).Select(p => p.ToPersonResponseDTO()).ToList();
        }

        public async Task<PersonResponseDTO> GetPersonById(Guid? personID)
        {
            if (personID == null)
            {
                throw new ArgumentNullException(nameof(personID), "Person ID cannot be null");
            }
            Person? newPerson = await _personRepository.GetPersonById(personID);
            if (newPerson == null)
            {
                throw new InvalidPersonIdException("Given person id doesn't exist");
            }
            else
            {
                return newPerson.ToPersonResponseDTO();
            }
        }

        public async Task<List<PersonResponseDTO>> GetFilteredPersons(string filterBy, string? filterString)
        {
            List<PersonResponseDTO> matchingPersons = new List<PersonResponseDTO>();
            List<PersonResponseDTO> persons = await GetAllPersons();
            if (string.IsNullOrEmpty(filterString) || string.IsNullOrEmpty(filterBy))
            {
                return persons;
            }

            switch (filterBy)
            {
                case nameof(PersonResponseDTO.PersonName):
                    {
                        matchingPersons = persons.Where(p => (p.PersonName != null) ? p.PersonName.Contains(filterString, StringComparison.OrdinalIgnoreCase) : false).ToList();
                        break;
                    }
                case nameof(PersonResponseDTO.Email):
                    {
                        matchingPersons = persons.Where(p => (p.Email != null) ? p.Email.ToString().Contains(filterString, StringComparison.OrdinalIgnoreCase) : false).ToList();
                        break;
                    }
                case nameof(PersonResponseDTO.DateOfBirth):
                    {
                        matchingPersons = persons.Where(p => (p.DateOfBirth != null) ? p.DateOfBirth.Value.ToString("MM dd yyyy").Contains(filterString, StringComparison.OrdinalIgnoreCase) : false).ToList();
                        break;
                    }
                case nameof(PersonResponseDTO.Age):
                    {
                        matchingPersons = persons.Where(p => (p.DateOfBirth != null) ? Math.Round((DateTime.Now - p.DateOfBirth.Value).TotalDays / 365.25) == double.Parse(filterString) : false).ToList();
                        break;
                    }
                case nameof(PersonResponseDTO.Gender):
                    {
                        matchingPersons = persons.Where(p => (p.Gender != null) ? p.Gender.Equals(filterString, StringComparison.OrdinalIgnoreCase) : false).ToList();
                        break;
                    }
                case nameof(PersonResponseDTO.Country):
                    {
                        matchingPersons = persons.Where(p => (p.Country != null) ? p.Country.Contains(filterString) : false).ToList();
                        break;
                    }
                default:
                    matchingPersons = persons;
                    break;
            }
            return matchingPersons;
        }

    }
}
