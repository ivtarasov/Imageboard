using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Netaba.Data.Enums;
using Netaba.Data.Models;
using Netaba.Data.ViewModels;
using Netaba.Services.Repository;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Netaba.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _repository;
        public AccountController(IUserRepository repository) => _repository = repository;

        [HttpGet]
        [Route("/login", Name = "Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("/login", Name = "Login")]
        public async Task<IActionResult> Login(Login login)
        {
            if (ModelState.IsValid)
            {
                var user = await _repository.FindUserAsync(login.Name, login.Password);
                if (user != null)
                {
                    await Authenticate(user);
                    return RedirectToRoute("Login");
                }
                ModelState.AddModelError("", "Invalid name and(or) password.");
            }

            return View(new LoginViewModel(login));
        }
        
        [HttpGet]
        [Route("/add_admin", Name = "AdminAdding")]
        [Authorize(Roles = nameof(Role.SuperAdmin))]
        public IActionResult AddAdmin()
        {
            return View();
        }

        [HttpPost]
        [Route("/add_admin", Name = "AdminAdding")]
        [Authorize(Roles = nameof(Role.SuperAdmin))]
        public async Task<IActionResult> AddAdmin(Register register)
        {
            if (ModelState.IsValid)
            {
                if (await _repository.FindUserAsync(register.Name) == null)
                {
                    var user = new User(register.Name, Role.Admin, register.Password);

                    var isSuccess = await _repository.TryAddUserAsync(user);
                    if (!isSuccess)
                    {
                        ModelState.AddModelError("", "Unable to add admin.");
                        return View(new RegisterViewModel(register));
                    }

                    return RedirectToRoute("AdminAdding");
                }
                else ModelState.AddModelError("", "Admin with this name already exists.");
            }
            return View(new RegisterViewModel(register));
        }

        [HttpGet]
        [Route("/del_admin", Name = "DeleteAdmin")]
        [Authorize(Roles = nameof(Role.SuperAdmin))]
        public IActionResult DeleteAdmin()
        {
            return View();
        }

        [HttpPost]
        [Route("/del_admin", Name = "AdminDeleting")]
        [Authorize(Roles = nameof(Role.SuperAdmin))]
        public async Task<IActionResult> DeleteAdmin([Required(ErrorMessage = "Name is not specified.")] string adminName)
        {
            if (ModelState.IsValid)
            {
                var ruser = await _repository.FindUserAsync(adminName);
                if (await _repository.FindUserAsync(adminName) != null)
                {
                    var isSuccess = await _repository.TryDeleteUserAsync(ruser);
                    if (!isSuccess)
                    {
                        ModelState.AddModelError("", "Unable to delete admin.");
                        View(new DeleteAdminViewModel(adminName));
                    }

                    return RedirectToRoute("AdminDeleting");
                }
                else ModelState.AddModelError("", "Admin with this name does not exist.");
            }
            return View(new DeleteAdminViewModel(adminName));
        }

        [HttpGet]
        [Route("/logout", Name = "Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToRoute("Login");
        }

        private async Task Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Name),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString())
            };

            ClaimsIdentity id = new(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}
