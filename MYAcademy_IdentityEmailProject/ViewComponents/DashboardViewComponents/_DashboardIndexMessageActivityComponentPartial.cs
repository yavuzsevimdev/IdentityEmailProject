using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MYAcademy_IdentityEmailProject.Context;
using MYAcademy_IdentityEmailProject.Entities;
using MYAcademy_IdentityEmailProject.Models;

namespace MYAcademy_IdentityEmailProject.ViewComponents.DashboardViewComponents
{
    public class _DashboardIndexMessageActivityComponentPartial : ViewComponent
    {
        private readonly EmailContext _context;
        private readonly UserManager<AppUser> _userManager;

        public _DashboardIndexMessageActivityComponentPartial(EmailContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(UserClaimsPrincipal);

            if (user == null || string.IsNullOrEmpty(user.Email))
            {
                return View(new DashboardMessageActivityViewModel());
            }

            var userEmail = user.Email;

            var endDate = DateTime.Today.AddDays(1);
            var startDate = endDate.AddDays(-7);

            var receivedData = await _context.Messages
                .AsNoTracking()
                .Where(x =>
                    x.ReceiverEmail == userEmail &&
                    !x.IsTrash &&
                    !x.IsDraft &&
                    x.SendDate >= startDate &&
                    x.SendDate < endDate)
                .GroupBy(x => x.SendDate.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();

            var sentData = await _context.Messages
                .AsNoTracking()
                .Where(x =>
                    x.SenderEmail == userEmail &&
                    !x.IsTrash &&
                    !x.IsDraft &&
                    x.SendDate >= startDate &&
                    x.SendDate < endDate)
                .GroupBy(x => x.SendDate.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();

            var receivedDictionary = receivedData.ToDictionary(x => x.Date, x => x.Count);

            var sentDictionary = sentData.ToDictionary(x => x.Date, x => x.Count);

            var model = new DashboardMessageActivityViewModel();

            for (int i = 0; i < 7; i++)
            {
                var date = startDate.AddDays(i);

                model.Labels.Add(date.ToString("dd MMM"));

                model.ReceivedCounts.Add(receivedDictionary.GetValueOrDefault(date.Date, 0));

                model.SentCounts.Add(sentDictionary.GetValueOrDefault(date.Date, 0));
            }

            model.TotalReceived = model.ReceivedCounts.Sum();
            model.TotalSent = model.SentCounts.Sum();

            return View(model);
        }
    }
}