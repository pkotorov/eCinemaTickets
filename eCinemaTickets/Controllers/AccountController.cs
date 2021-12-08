using eCinemaTickets.Data;
using eCinemaTickets.Data.Static;
using eCinemaTickets.Data.ViewModels;
using eCinemaTickets.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCinemaTickets.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly AppDbContext context;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AppDbContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context;
        }

        public async Task<IActionResult> Users()
        {
            var users = await this.context.Users.ToListAsync();

            return this.View(users);
        }

        public IActionResult Login()
        {
            var response = new LoginViewModel();

            return this.View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(loginViewModel);
            }

            var user = await this.userManager.FindByEmailAsync(loginViewModel.EmailAddress);

            if (user != null)
            {
                var passwordCheck = await this.userManager.CheckPasswordAsync(user, loginViewModel.Password);

                if (passwordCheck)
                {
                    var result = await this.signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);

                    if (result.Succeeded)
                    {
                        return this.RedirectToAction("Index", "Movies");
                    }
                }

                TempData["Error"] = "Wrong credentials. Please try again!";

                return this.View(loginViewModel);
            }

            TempData["Error"] = "Wrong credentials. Please try again!";

            return this.View(loginViewModel);
        }

        public IActionResult Register()
        {
            var response = new RegisterViewModel();

            return this.View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(registerViewModel);
            }

            var user = await this.userManager.FindByEmailAsync(registerViewModel.EmailAddress);

            if (user != null)
            {
                TempData["Error"] = "This email address is already taken.";
                return this.View(registerViewModel);
            }

            var newUser = new ApplicationUser()
            {
                FullName = registerViewModel.FullName,
                Email = registerViewModel.EmailAddress,
                UserName = registerViewModel.EmailAddress,
            };

            var newUserReponse = await this.userManager.CreateAsync(newUser, registerViewModel.Password);

            if (newUserReponse.Succeeded)
            {
                await this.userManager.AddToRoleAsync(newUser, UserRoles.User);
            }

            return this.View("RegisterCompleted");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await this.signInManager.SignOutAsync();

            return this.RedirectToAction("Index", "Movies");
        }

        public IActionResult AccessDenied(string returnUrl)
        {
            return this.View();
        }
    }
}
