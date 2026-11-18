using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MYAcademy_IdentityEmailProject.Context;
using MYAcademy_IdentityEmailProject.Entities;
using MYAcademy_IdentityEmailProject.Models;

namespace MYAcademy_IdentityEmailProject.ViewComponents.DashboardViewComponents
{
    public class _DashboardIndexKPICardsComponentPartial : ViewComponent
    {
        private readonly EmailContext _context;
        private readonly UserManager<AppUser> _userManager;

        public _DashboardIndexKPICardsComponentPartial(EmailContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(UserClaimsPrincipal);

            if (user == null || string.IsNullOrEmpty(user.Email))
            {
                return View(new DashboardKpiViewModel());
            }

            var userEmail = user.Email;

            var receivedQuery = _context.Messages
                .AsNoTracking()
                .Where(x =>
                    x.ReceiverEmail == userEmail &&
                    !x.IsTrash &&
                    !x.IsDraft);

            var sentQuery = _context.Messages
                .AsNoTracking()
                .Where(x =>
                    x.SenderEmail == userEmail &&
                    !x.IsTrash &&
                    !x.IsDraft);

            var model = new DashboardKpiViewModel
            {
                ReceivedCount = await receivedQuery.CountAsync(),

                SentCount = await sentQuery.CountAsync(),

                UnreadCount = await receivedQuery.CountAsync(x => !x.IsRead),

                FavoriteCount = await _context.Messages
                    .AsNoTracking()
                    .CountAsync(x =>
                        (x.SenderEmail == userEmail ||
                         x.ReceiverEmail == userEmail) &&
                        x.IsFavorite &&
                        !x.IsTrash),

                DraftCount = await _context.Messages
                    .AsNoTracking()
                    .CountAsync(x =>
                        x.SenderEmail == userEmail &&
                        x.IsDraft &&
                        !x.IsTrash)
            };

            return View(model);
        }
    }
}