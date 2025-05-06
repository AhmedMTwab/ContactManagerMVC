using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SerivicesContracts.DTO;
using SerivicesContracts.Enums;

namespace ContactManager.CORE.ServiciesContracts.IPersonServices
{
    public interface IPersonSorter
    {
        public List<PersonResponseDTO> GetSortedPersons(List<PersonResponseDTO> UnSortedPersons, string sortBy, SortOrderEnum sortOrder);

    }
}
