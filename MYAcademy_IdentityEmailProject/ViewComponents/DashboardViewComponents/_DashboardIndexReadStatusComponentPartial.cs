using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MYAcademy_IdentityEmailProject.Context;
using MYAcademy_IdentityEmailProject.Entities;
using MYAcademy_IdentityEmailProject.Models;

namespace MYAcademy_IdentityEmailProject.ViewComponents.DashboardViewComponents
{
    public class _DashboardIndexReadStatusComponentPartial : ViewComponent
    {
        private readonly EmailContext _context;
        private readonly UserManager<AppUser> _userManager;

        public _DashboardIndexReadStatusComponentPartial(EmailContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new DashboardReadStatusViewModel();

            var user = await _userManager.GetUserAsync(UserClaimsPrincipal);

            if (user == null || string.IsNullOrWhiteSpace(user.Email))
                return View(model);

            var userEmail = user.Email;

            var readStatus = await _context.Messages
                .AsNoTracking()
                .Where(x =>
                    x.ReceiverEmail == userEmail &&
                    !x.IsTrash &&
                    !x.IsDraft)
                .GroupBy(x => x.IsRead)
                .Select(g => new
                {
                    IsRead = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();

            model.ReadCount = readStatus
                .Where(x => x.IsRead)
                .Select(x => x.Count)
                .FirstOrDefault();

            model.UnreadCount = readStatus
                .Where(x => !x.IsRead)
                .Select(x => x.Count)
                .FirstOrDefault();

            return View(model);
        }
    }
}