using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SerivicesContracts.DTO;

namespace ContactManager.CORE.ServiciesContracts.IContryServices
{
    public  interface ICountryGetter
    {
        /// <summary>
        /// get all countries data
        /// </summary>
        /// <returns>collection of countryResposeDTO contains countries data</returns>
        public Task<List<CountryResponseDTO>> GetAllCountries();
        /// <summary>
        /// get country by its countryId
        /// </summary>
        /// <param name="id">Guid countryId</param>
        /// <returns>object of countryResponseDTO contains country data</returns>
        public Task<CountryResponseDTO?> GetCountryByID(Guid id);
        /// <summary>
        /// get country by its countryName
        /// </summary>
        /// <param name="Name">Guid countryName</param>
        /// <returns>object of countryResponseDTO contains country data</returns>
        public Task<CountryResponseDTO?> GetCountryByName(string countryName);

    }
}
