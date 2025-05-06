using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactManager.CORE.ServiciesContracts.IPersonServices;
using Entities;
using RepositoryContract;

namespace ContactManager.CORE.Servicies.PersonServices
{
    public class PersonDeleter:IPersonDeleter
    {
        private readonly IPersonRepository _personRepository;
        
        //constructor
        public PersonDeleter(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<bool> DeletePerson(Guid? personId)
        {
            if (personId == null)
            {
                throw new ArgumentNullException(nameof(personId), "Person ID cannot be null");
            }
            Person? personToDelete = await _personRepository.GetPersonById(personId);
            if (personToDelete == null)
            {
                return false;
            }
            else
            {
                await _personRepository.DeletePerson(personToDelete);
                return true;
            }
        }

    }
}
