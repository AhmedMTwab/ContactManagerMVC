using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactManager.CORE.DTO;

namespace ContactManager.CORE.ServiciesContracts.IIdentityServices
{
    public interface IUserRegisterService
    {
        public Task<UserRegisterDTO> UserRegister(UserRegisterDTO userRegisterDTO);
    }
}
