using ContactManager.CORE.Domain.Entities.IdentityEntites;
using ContactManager.CORE.DTO;
using ContactManager.CORE.ServiciesContracts.IIdentityServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ContactManager.UI.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IUserRegisterService _userRegisterService;
        private readonly IUserLoginService _userLoginService;
        private readonly IUserLogoutService _userLogoutService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICurrentApplicationUserService _currentApplicationUserService;

        public AccountController(IUserRegisterService userRegisterService,IUserLoginService userLoginService,IUserLogoutService userLogoutService,UserManager<ApplicationUser> userManager,ICurrentApplicationUserService currentApplicationUserService) 
        {
            this._userRegisterService = userRegisterService;
            this._userLoginService = userLoginService;
            this._userLogoutService = userLogoutService;
            this._userManager = userManager;
            this._currentApplicationUserService = currentApplicationUserService;
        }
      
        [HttpGet]
        [Route("/Account/Register")]
        public IActionResult Register()
        {
            return View("Register");
        }
        [HttpPost]
        [Route("/Account/Register")]
        public async Task<IActionResult> Register(UserRegisterDTO userRegister)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _userRegisterService.UserRegister(userRegister);
                    return RedirectToAction("Index", "Persons");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(userRegister);
        }
        [HttpGet]
        [Route("/Account/Login")]
        public IActionResult Login()
        {
            return View("Login");
        }
        [HttpPost]
        [Route("/Account/Login")]
        public async  Task<IActionResult> Login(UserLoginDTO userLoginDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _userLoginService.LoginUser(userLoginDTO);
                    var currentUser = await _userManager.GetUserAsync(User);
                    _currentApplicationUserService.GetCurrentApplicationUserId(currentUser);
                    return RedirectToAction(nameof(Index),"Persons");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(userLoginDTO);
        }
        [Route("/Account/Logout")]
        [Authorize]
        public IActionResult Logout()
        {
           _userLogoutService.LogoutUser();
            return RedirectToAction("Index", "Persons");
        }

    }
}
