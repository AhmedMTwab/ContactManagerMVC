using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactManager.CORE.Domain.Entities.IdentityEntites;
using ContactManager.CORE.ServiciesContracts.IIdentityServices;
using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RepositoryContract;

namespace Repository
{
    public class PersonRepository : IPersonRepository
    {
        private readonly ContactManagerDbContext _db;
      
        public PersonRepository(ContactManagerDbContext dbcontext)
        {
            this._db = dbcontext;
        }   
        public async Task<Person> AddPerson(Person newPerson)
        {
            _db.persons.Add(newPerson);
            await _db.SaveChangesAsync();
            return newPerson;
        }

        public async Task<bool> DeletePerson(Person? person)
        {
            _db.persons.Remove(person);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<List<Person>> GetAllPersons()
        {
           List<Person> persons= await _db.persons.Include(p=>p.ApplicationUser).Include(c=>c.Country).ToListAsync();
            return persons; 
        }

        public async Task<Person> GetPersonById(Guid? personId)
        {
           Person? person=await _db.persons.Include(p => p.ApplicationUser).Include(p=>p.Country).FirstOrDefaultAsync(p => p.PersonID == personId);
           
            return person;
        }
        public async Task<Person> UpdatePerson(Person person)
        {
            //update all details
            Person? matchingPerson = await GetPersonById(person.PersonID);
            if (matchingPerson == null)
            {
                throw new ArgumentException("Given person id doesn't exist");
            }
            matchingPerson.PersonName = person.PersonName;
            matchingPerson.Email = person.Email;
            matchingPerson.DateOfBirth = person.DateOfBirth;
            matchingPerson.Gender = person.Gender.ToString();
            matchingPerson.CountryID = person.CountryID;
            matchingPerson.Address = person.Address;
            matchingPerson.phoneNumber = person.phoneNumber;
            await _db.SaveChangesAsync();
            return matchingPerson;
        }
    }
}
