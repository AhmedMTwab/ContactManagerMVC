using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactManager.CORE.Domain.Entities.IdentityEntites;

namespace ContactManager.CORE.ServiciesContracts.IIdentityServices
{
    public interface ICurrentApplicationUserService
    {
        public Guid currentApplicationUserId { get; set; }
        public void GetCurrentApplicationUserId(ApplicationUser applicationUser);
    }
}
