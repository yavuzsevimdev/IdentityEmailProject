using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MYAcademy_IdentityEmailProject.Entities;
using MYAcademy_IdentityEmailProject.Models;

namespace MYAcademy_IdentityEmailProject.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public ProfileController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> MyProfile()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("SignIn", "Login");

            var model = new MyProfileViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Description = user.Description,
                ImageUrl = user.ImageUrl,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(
            MyProfileViewModel model)
        {
            // Profil bilgilerini yeniden yükleyeceğiz.
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("SignIn", "Login");

            if (!ModelState.IsValid)
            {
                model.Id = user.Id;
                model.Name = user.Name;
                model.Surname = user.Surname;
                model.UserName = user.UserName;
                model.Email = user.Email;
                model.PhoneNumber = user.PhoneNumber;
                model.Description = user.Description;
                model.ImageUrl = user.ImageUrl;
                model.EmailConfirmed = user.EmailConfirmed;
                model.PhoneNumberConfirmed = user.PhoneNumberConfirmed;

                return View("MyProfile", model);
            }

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Şifreniz başarıyla değiştirildi.";

                return RedirectToAction(nameof(MyProfile));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("CurrentPassword", error.Description);
            }

            model.Id = user.Id;
            model.Name = user.Name;
            model.Surname = user.Surname;
            model.UserName = user.UserName;
            model.Email = user.Email;
            model.PhoneNumber = user.PhoneNumber;
            model.Description = user.Description;
            model.ImageUrl = user.ImageUrl;
            model.EmailConfirmed = user.EmailConfirmed;
            model.PhoneNumberConfirmed = user.PhoneNumberConfirmed;

            return View("MyProfile", model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(MyProfileViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("SignIn", "Login");

            ModelState.Remove(nameof(model.CurrentPassword));
            ModelState.Remove(nameof(model.NewPassword));
            ModelState.Remove(nameof(model.ConfirmPassword));

            if (!ModelState.IsValid)
            {
                model.Id = user.Id;
                model.Email = user.Email;
                model.EmailConfirmed = user.EmailConfirmed;
                model.PhoneNumberConfirmed = user.PhoneNumberConfirmed;

                return View("MyProfile", model);
            }

            user.Name = model.Name;
            user.Surname = model.Surname;
            user.Description = model.Description;
            user.ImageUrl = model.ImageUrl;

            var userNameResult = await _userManager.SetUserNameAsync(user, model.UserName);

            if (!userNameResult.Succeeded)
            {
                foreach (var error in userNameResult.Errors)
                    ModelState.AddModelError(nameof(model.UserName), error.Description);

                model.Id = user.Id;
                model.Email = user.Email;
                model.EmailConfirmed = user.EmailConfirmed;
                model.PhoneNumberConfirmed = user.PhoneNumberConfirmed;

                return View("MyProfile", model);
            }

            var phoneResult = await _userManager.SetPhoneNumberAsync(user, model.PhoneNumber);

            if (!phoneResult.Succeeded)
            {
                foreach (var error in phoneResult.Errors)
                    ModelState.AddModelError(nameof(model.PhoneNumber), error.Description);

                model.Id = user.Id;
                model.Email = user.Email;
                model.EmailConfirmed = user.EmailConfirmed;
                model.PhoneNumberConfirmed = user.PhoneNumberConfirmed;

                return View("MyProfile", model);
            }

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);

                model.Id = user.Id;
                model.Email = user.Email;
                model.EmailConfirmed = user.EmailConfirmed;
                model.PhoneNumberConfirmed = user.PhoneNumberConfirmed;

                return View("MyProfile", model);
            }

            TempData["SuccessMessage"] = "Profil bilgileriniz başarıyla güncellendi.";

            return RedirectToAction(nameof(MyProfile));
        }
    }
}
