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
        [ValidateAntiForgeryToken]
        [Route("/login", Name = "Login")]
        public async Task<IActionResult> Login(Login login)
        {
            if (ModelState.IsValid)
            {
                var user = await _repository.FindUser(login.Name, login.Password);
                if (user != null)
                {
                    await Authenticate(user);
                    return Content($"Success.");
                }
                ModelState.AddModelError("", "Invalid name and(or) password.");
            }

            return View(new LoginViewModel(login));
        }
        
        [HttpGet]
        [Route("/register", Name = "Register")]
        [Authorize(Roles = nameof(Role.SuperAdmin))]
        public IActionResult RegisterNewAdmin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/register", Name = "Register")]
        [Authorize(Roles = nameof(Role.SuperAdmin))]
        public async Task<IActionResult> RegisterNewAdmin(Register register)
        {
            if (ModelState.IsValid)
            {
                if (await _repository.FindUser(register.Name) == null)
                {
                    var user = new User(register.Name, Role.Admin, register.Password);
                    await _repository.TryAddUser(user);
                    await Authenticate(user);

                    return Content($"Success.");
                }
                else
                    ModelState.AddModelError("", "Admin with this name already exists.");
            }
            return View(new RegisterViewModel(register));
        }

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
