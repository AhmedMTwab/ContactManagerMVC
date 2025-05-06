using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactManager.CORE.Domain.Entities.IdentityEntites;
using ContactManager.CORE.ServiciesContracts.IIdentityServices;

namespace ContactManager.CORE.Services.PersonsServices
{
    public  class CurrentApplicationUserService : ICurrentApplicationUserService
    {
        public Guid currentApplicationUserId { get; set; }
        public void GetCurrentApplicationUserId(ApplicationUser applicationUser)
        {
            if (applicationUser != null)
            {
                currentApplicationUserId= applicationUser.Id;
            }
            else
            {
                throw new ArgumentNullException(nameof(applicationUser), "Application user cannot be null");
            }
        }
    }
   
}
