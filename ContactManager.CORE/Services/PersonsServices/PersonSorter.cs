using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactManager.CORE.ServiciesContracts.IPersonServices;
using RepositoryContract;
using SerivicesContracts.DTO;
using SerivicesContracts.Enums;

namespace ContactManager.CORE.Services.PersonsServices
{
    public class PersonSorter:IPersonSorter
    {
        private readonly IPersonRepository _personRepository;
        
        //constructor
        public PersonSorter(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public List<PersonResponseDTO> GetSortedPersons(List<PersonResponseDTO> UnSortedPersons, string sortBy, SortOrderEnum sortOrder)
        {
            List<PersonResponseDTO> sortedPersonsList = UnSortedPersons;
            sortedPersonsList = (sortBy, sortOrder) switch
            {
                (nameof(PersonResponseDTO.PersonName), SortOrderEnum.Ascending) => UnSortedPersons.OrderBy(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponseDTO.PersonName), SortOrderEnum.Descending) => UnSortedPersons.OrderByDescending(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponseDTO.Email), SortOrderEnum.Ascending) => UnSortedPersons.OrderBy(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponseDTO.Email), SortOrderEnum.Descending) => UnSortedPersons.OrderByDescending(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponseDTO.DateOfBirth), SortOrderEnum.Ascending) => UnSortedPersons.OrderBy(temp => temp.DateOfBirth).ToList(),

                (nameof(PersonResponseDTO.DateOfBirth), SortOrderEnum.Descending) => UnSortedPersons.OrderByDescending(temp => temp.DateOfBirth).ToList(),

                (nameof(PersonResponseDTO.Age), SortOrderEnum.Ascending) => UnSortedPersons.OrderBy(temp => temp.Age).ToList(),

                (nameof(PersonResponseDTO.Age), SortOrderEnum.Descending) => UnSortedPersons.OrderByDescending(temp => temp.Age).ToList(),

                (nameof(PersonResponseDTO.Gender), SortOrderEnum.Ascending) => UnSortedPersons.OrderBy(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponseDTO.Gender), SortOrderEnum.Descending) => UnSortedPersons.OrderByDescending(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponseDTO.Country), SortOrderEnum.Ascending) => UnSortedPersons.OrderBy(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponseDTO.Country), SortOrderEnum.Descending) => UnSortedPersons.OrderByDescending(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponseDTO.Address), SortOrderEnum.Ascending) => UnSortedPersons.OrderBy(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponseDTO.Address), SortOrderEnum.Descending) => UnSortedPersons.OrderByDescending(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponseDTO.phoneNumber), SortOrderEnum.Ascending) => UnSortedPersons.OrderBy(temp => temp.phoneNumber).ToList(),

                (nameof(PersonResponseDTO.phoneNumber), SortOrderEnum.Descending) => UnSortedPersons.OrderByDescending(temp => temp.phoneNumber).ToList(),

                _ => UnSortedPersons
            };
            return sortedPersonsList;
        }

    }
}
