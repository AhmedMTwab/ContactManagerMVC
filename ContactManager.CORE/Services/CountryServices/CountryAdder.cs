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
    public class CountryAdder : ICountryAdder
    {
        private readonly ICountryRepository _countryRepository;

        //constructor

        public CountryAdder(ICountryRepository countryRepository)
        {
            this._countryRepository = countryRepository;
        }

        public async Task<CountryResponseDTO> AddCountry(CountryAddDTO countryAddRequest)
        {
            if (countryAddRequest == null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest));
            }
            if (countryAddRequest.CountryName == null)
            {
                throw new ArgumentException("Country name cannot be null");
            }
            if (await _countryRepository.GetCountrybyName(countryAddRequest.CountryName) != null)
            {
                throw new ArgumentException("Country Name already exist");
            }
            Country country = countryAddRequest.ToCountry();
            country.CountryId = Guid.NewGuid();
            await _countryRepository.AddCountry(country);
            return new CountryResponseDTO
            {
                CountryId = country.CountryId,
                CountryName = country.CountryName
            };

        }

    }
}
