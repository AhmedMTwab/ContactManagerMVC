using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace SerivicesContracts.DTO
{
    public class CountryResponseDTO
    {
        public Guid CountryId { get; set; }
        public string CountryName { get; set; }

        public override bool Equals(object? obj)
        {
            if(obj ==null)
            {
                return false;
            }
            if(obj.GetType() != typeof(CountryResponseDTO))
            {
                return false;
            }
            CountryResponseDTO comparedObj= obj as CountryResponseDTO;

            return this.CountryId == comparedObj.CountryId;
        }
    }
    public static class CountryResponseExtensions
    {
        public static Country ToCountry(this CountryResponseDTO countryResponse)
        {
            return new Country
            {
                CountryId = countryResponse.CountryId,
                CountryName = countryResponse.CountryName
            };
        }
        public static CountryResponseDTO ToCountryResponseDTO(this Country country)
        {
            if(country == null)
            {
                return null;
            }
            return new CountryResponseDTO
            {
                CountryId = country.CountryId,
                CountryName = country.CountryName
            };
        }
    }
}
