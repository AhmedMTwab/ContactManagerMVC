using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactManager.CORE.Domain.Entities.IdentityEntites;
using ContactManager.CORE.DTO;
using ContactManager.CORE.ServiciesContracts.IIdentityServices;
using Microsoft.AspNetCore.Identity;
using Services.Helpers;

namespace ContactManager.CORE.Services.IdentityServices
{
    public class UserRegisterService: IUserRegisterService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserRegisterService(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            this._signInManager = signInManager;
        }
        public async Task<UserRegisterDTO> UserRegister(UserRegisterDTO userRegisterDTO)
        {
            ValidationHelper.Validate(userRegisterDTO);
            var user = new ApplicationUser
            {
                UserName = userRegisterDTO.UserName,
                Email = userRegisterDTO.Email,
                PersonName = userRegisterDTO.PersonName,
                DateOfBirth = userRegisterDTO.DateOfBirth,
                PhoneNumber = userRegisterDTO.PhoneNumber,
              
            };
            IdentityResult registerResult=await _userManager.CreateAsync(user,userRegisterDTO.Password);
            if (registerResult.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return userRegisterDTO;
            }
            else
            {
                var errors = registerResult.Errors.Select(e => e.Description).ToList();
                throw new Exception(string.Join(", ", errors));
            }
        }
    }
   
    
}
