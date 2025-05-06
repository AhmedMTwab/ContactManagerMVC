using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using RepositoryContract;
using SerivicesContracts.DTO;

namespace ContactManager.CORE.ServiciesContracts.IContryServices
{
    public class ConuntryGetter : ICountryGetter
    {
        private readonly ICountryRepository _countryRepository;
        public ConuntryGetter(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<List<CountryResponseDTO>> GetAllCountries()
        {
            List<CountryResponseDTO> GetAllCountriesResult = new List<CountryResponseDTO>();
            foreach (Country country in await _countryRepository.GetAllCountries())
            {
                CountryResponseDTO countryResponseFormCountryObj = country.ToCountryResponseDTO();
                GetAllCountriesResult.Add(countryResponseFormCountryObj);
            }
            return GetAllCountriesResult;
        }

        public async Task<CountryResponseDTO?> GetCountryByID(Guid countryId)
        {
            if (countryId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(countryId));
            }
            Country? Resultcountry = await _countryRepository.GetCountrybyId(countryId);
            CountryResponseDTO getCountryResponse = Resultcountry != null ? Resultcountry.ToCountryResponseDTO() : throw new ArgumentNullException(nameof(Resultcountry)); ;
            return getCountryResponse;
        }

        public async Task<CountryResponseDTO> GetCountryByName(string countryName)
        {
            Country country = await _countryRepository.GetCountrybyName(countryName);
            if (country == null)
            {
                throw new ArgumentNullException(nameof(country));
            }
            CountryResponseDTO getCountryResponse = country.ToCountryResponseDTO();
            return getCountryResponse;
        }
    

}
}
