using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MYAcademy_IdentityEmailProject.Entities;
using MYAcademy_IdentityEmailProject.Models;

namespace MYAcademy_IdentityEmailProject.ViewComponents.MessageLayoutViewComponents
{
    public class _MessageLayoutNavbarComponentPartial : ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;

        public _MessageLayoutNavbarComponentPartial(
            UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(UserClaimsPrincipal);

            if (user == null)
                return View(null);

            var fullName = $"{user.Name} {user.Surname}".Trim();

            if (string.IsNullOrWhiteSpace(fullName))
                fullName = user.UserName ?? "Kullanıcı";

            var initials = string.Concat(
                user.Name?.FirstOrDefault(),
                user.Surname?.FirstOrDefault()
            ).ToUpper();

            if (string.IsNullOrWhiteSpace(initials))
                initials = user.UserName?.Length > 0 ? user.UserName.Substring(0, 1).ToUpper() : "K";

            var model = new NavbarUserViewModel
            {
                FullName = fullName,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                ImageUrl = user.ImageUrl,
                Initials = initials
            };

            return View(model);
        }
    }
}