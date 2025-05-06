using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManager.CORE.ServiciesContracts.IIdentityServices
{
    public interface IUserLogoutService
    {
        public Task LogoutUser();
    }
}
