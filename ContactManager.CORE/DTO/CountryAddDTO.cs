using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace SerivicesContracts.DTO
{
    public class CountryAddDTO
    {
        public string CountryName { get; set; }

        
    }
    public static class CountryAddDTOExtensions
    {
        public static Country ToCountry(this CountryAddDTO countryAddDTO)
        {
            return new Country
            {
                CountryName = countryAddDTO.CountryName
            };
        }
    }
}
