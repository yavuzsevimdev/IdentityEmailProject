using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MYAcademy_IdentityEmailProject.Entities;
using MYAcademy_IdentityEmailProject.Models;
using MYAcademy_IdentityEmailProject.Services;

namespace MYAcademy_IdentityEmailProject.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;

        public RegisterController(
            UserManager<AppUser> userManager,
            IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(
            UserRegisterViewModel userRegisterViewModel)
        {
            if (!ModelState.IsValid)
                return View(userRegisterViewModel);

            Random random = new Random();
            int validCode = random.Next(100000, 1000000);

            AppUser appUser = new AppUser()
            {
                Name = userRegisterViewModel.Name,
                Surname = userRegisterViewModel.Surname,
                Email = userRegisterViewModel.Email,
                UserName = userRegisterViewModel.Username,
                ValidCode = validCode
            };

            var result = await _userManager.CreateAsync(
                appUser,
                userRegisterViewModel.Password);

            if (result.Succeeded)
            {
                await _emailService.SendEmailAsync(
                    userRegisterViewModel.Email,
                    "Email Doğrulama Kodu",
                    "Sisteme giriş yapabilmek için gerekli olan doğrulama kodu: " + validCode);

                return RedirectToAction("SignIn", "Login");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(userRegisterViewModel);
        }
    }
}