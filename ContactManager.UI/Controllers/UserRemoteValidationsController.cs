using ContactManager.CORE.Domain.Entities.IdentityEntites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ContactManager.UI.Controllers
{
    [AllowAnonymous]
    public class UserRemoteValidationsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRemoteValidationsController(UserManager<ApplicationUser> userManager)
        {
            this._userManager = userManager;
        }
        [Route("/UserRemoteValidationsController/EmailIsExist")]
        public async Task<IActionResult> EmailIsExist(string Email)
        {
            if (await _userManager.FindByEmailAsync(Email) == null)
                return Json(true);
            else
                return Json(false);
        }
        [Route("/UserRemoteValidationsController/UserNameIsExist")]
        public async Task<IActionResult> UserNameIsExist(string UserName)
        {
            if (await _userManager.FindByNameAsync(UserName) == null)
                return Json(true);
            else
                return Json(false);
        }
    }
}
