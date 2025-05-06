using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactManager.CORE.Domain.Entities.IdentityEntites;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class ContactManagerDbContext:IdentityDbContext<ApplicationUser,ApplicationRole,Guid>
    {
        public ContactManagerDbContext() : base()
        {
        }
        public ContactManagerDbContext(DbContextOptions options): base(options)
        {
        }
        public virtual DbSet<Person> persons { get; set; }
        public virtual DbSet<Country> countries { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Country>().ToTable("Countries").HasKey(c=>c.CountryId);
            modelBuilder.Entity<Person>().ToTable("Persons").HasKey(p => p.PersonID);

        }

    }
}
