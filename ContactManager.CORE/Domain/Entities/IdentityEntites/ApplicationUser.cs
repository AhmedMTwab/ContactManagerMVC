using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Microsoft.AspNetCore.Identity;

namespace ContactManager.CORE.Domain.Entities.IdentityEntites
{
    public  class ApplicationUser:IdentityUser<Guid>
    {
        public string PersonName { get; set; }
        public DateTime DateOfBirth { get; set; }

        public virtual ICollection<Person>? Persons { get; set; }

    }
}
