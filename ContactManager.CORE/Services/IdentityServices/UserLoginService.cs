using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactManager.CORE.Domain.Entities.IdentityEntites;
using ContactManager.CORE.DTO;
using ContactManager.CORE.ServiciesContracts.IIdentityServices;
using Microsoft.AspNetCore.Identity;

namespace ContactManager.CORE.Services.IdentityServices
{
    public class UserLoginService: IUserLoginService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserLoginService(SignInManager<ApplicationUser> signInManager,UserManager<ApplicationUser> userManager)
        {
            this._signInManager = signInManager;
            this._userManager = userManager;
        }
        public async Task<UserLoginDTO> LoginUser(UserLoginDTO userLoginDTO)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(userLoginDTO.Email);

            var Result =await  _signInManager.PasswordSignInAsync(user,userLoginDTO.Password,isPersistent:userLoginDTO.RememberMe,false);

            if (Result.Succeeded)
            {
                return userLoginDTO;
            }
            else
            {
                throw new Exception("Invalid login attempt");
            }
        }
    }
  
}
