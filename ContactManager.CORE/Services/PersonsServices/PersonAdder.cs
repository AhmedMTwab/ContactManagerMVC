using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactManager.CORE.Helpers;
using ContactManager.CORE.ServiciesContracts.IIdentityServices;
using ContactManager.CORE.ServiciesContracts.IPersonServices;
using Entities;
using RepositoryContract;
using SerivicesContracts.DTO;
using Services.Helpers;

namespace ContactManager.CORE.Servicies.PersonServices
{
    public class PersonAdder:IPersonAdder
    {
        private readonly IPersonRepository _personRepository;
        private readonly ICurrentApplicationUserService _currentApplicationUserService;

        //constructor
        public PersonAdder(IPersonRepository personRepository,ICurrentApplicationUserService currentApplicationUserService)
        {
            _personRepository = personRepository;
            this._currentApplicationUserService = currentApplicationUserService;
        }


        public async Task<PersonResponseDTO> AddPerson(PersonAddDTO person)
        {
            if (person == null)
            {
                throw new ArgumentNullException(nameof(person), "Person cannot be null");

            }
            ValidationHelper.Validate(person);
            Person newPerson = person.ToPerson();
            newPerson.PersonID = Guid.NewGuid();
            newPerson.ApplicationUserID =_currentApplicationUserService.currentApplicationUserId;
            await _personRepository.AddPerson(newPerson);
            return newPerson.ToPersonResponseDTO();
        }


    }
}
