using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactManager.CORE.Domain.Entities.IdentityEntites;
using ContactManager.CORE.ServiciesContracts.IIdentityServices;
using Microsoft.AspNetCore.Identity;

namespace ContactManager.CORE.Services.IdentityServices
{
    public class UserLogoutService : IUserLogoutService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserLogoutService(SignInManager<ApplicationUser> signInManager)
        {
            this._signInManager = signInManager;
        }
        public async Task LogoutUser()
        {
           await _signInManager.SignOutAsync();
        }
       
    }
    
}
