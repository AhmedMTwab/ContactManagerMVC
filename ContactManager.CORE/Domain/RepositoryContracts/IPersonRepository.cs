using Entities;

namespace RepositoryContract
{
    public interface IPersonRepository
    {
        public Task<Person> AddPerson(Person newPerson);
        public Task<List<Person>> GetAllPersons();
        public Task<Person> GetPersonById(Guid? personId);

        public Task<Person> UpdatePerson(Person person);
        public Task<bool> DeletePerson(Person? person);
    }
}
