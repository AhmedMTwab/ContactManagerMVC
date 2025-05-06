using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SerivicesContracts.DTO;

namespace ContactManager.CORE.ServiciesContracts.IContryServices
{
    public  interface ICountryAdder
    {
        /// <summary>
        /// add new country to country table
        /// </summary>
        /// <param name="countryAddRequest">country data in countryAddRequest object</param>
        /// <returns>object of countryResponseDto with added country data</returns>
        public Task<CountryResponseDTO> AddCountry(CountryAddDTO countryAddRequest);

    }
}
