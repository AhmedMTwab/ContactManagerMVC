using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactManager.CORE.ServiciesContracts.IPersonServices;
using Entities;
using RepositoryContract;
using SerivicesContracts.DTO;
using Services.Helpers;

namespace ContactManager.CORE.Services.PersonServices
{
    public class PersonUpdater:IPersonUpdater
    {
        private readonly IPersonRepository _personRepository;
        
        //constructor
        public PersonUpdater(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }


        public async Task<PersonResponseDTO> UpdatePerson(PersonUpdateDTO person)
        {
            if (person == null)
                throw new ArgumentNullException(nameof(Person));

            //validation
            ValidationHelper.Validate(person);

            Person matchingPerson = await _personRepository.UpdatePerson(person.ToPerson());
            return matchingPerson.ToPersonResponseDTO();
        }

    }
}
