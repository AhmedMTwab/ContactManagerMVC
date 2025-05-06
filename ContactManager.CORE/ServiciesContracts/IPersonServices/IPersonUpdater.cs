using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SerivicesContracts.DTO;

namespace ContactManager.CORE.ServiciesContracts.IPersonServices
{
    public interface IPersonUpdater
    {
        public Task<PersonResponseDTO> UpdatePerson(PersonUpdateDTO person);

    }
}
