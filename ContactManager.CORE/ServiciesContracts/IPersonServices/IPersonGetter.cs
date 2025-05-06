using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SerivicesContracts.DTO;

namespace ContactManager.CORE.ServiciesContracts.IPersonServices
{
    public interface IPersonGetter
    {
        public Task<List<PersonResponseDTO>> GetAllPersons();
        public Task<PersonResponseDTO> GetPersonById(Guid? personID);
        public Task<List<PersonResponseDTO>> GetFilteredPersons(string filterBy, string? filterString);

    }
}
