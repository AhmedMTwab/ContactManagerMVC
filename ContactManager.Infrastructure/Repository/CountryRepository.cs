using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContract;

namespace Repository
{
    public class CountryRepository : ICountryRepository
    {
        private  ContactManagerDbContext _db;

        public CountryRepository(ContactManagerDbContext dbcontext)
        {
            this._db = dbcontext;
        }
        public async Task<Country> AddCountry(Country newCountry)
        {
            _db.countries.Add(newCountry);
            await _db.SaveChangesAsync();
            return newCountry;
        }

        public async Task<List<Country>> GetAllCountries()
        {
            List<Country> countries =await _db.countries.ToListAsync();
            return countries;
        }

        public async Task<Country?> GetCountrybyId(Guid? countryId)
        {
           Country? country =await _db.countries.FirstOrDefaultAsync(c => c.CountryId == countryId);
            return country;
        }

        public async Task<Country?> GetCountrybyName(string? countryName)
        {
            Country? country=await _db.countries.FirstOrDefaultAsync(c => c.CountryName == countryName);
            return country;
        }
    }
}
