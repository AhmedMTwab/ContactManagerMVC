using Entities;

namespace RepositoryContract
{
    public interface ICountryRepository
    {
        public  Task<Country> AddCountry(Country newCountry);
        public  Task<List<Country>> GetAllCountries();
        public  Task<Country> GetCountrybyId(Guid? countryId);
        public  Task<Country> GetCountrybyName(string? countryName);
    }
}
