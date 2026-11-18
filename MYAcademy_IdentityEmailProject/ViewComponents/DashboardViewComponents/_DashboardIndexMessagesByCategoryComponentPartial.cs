using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MYAcademy_IdentityEmailProject.Context;
using MYAcademy_IdentityEmailProject.Entities;
using MYAcademy_IdentityEmailProject.Models;

namespace MYAcademy_IdentityEmailProject.ViewComponents.DashboardViewComponents
{
    public class _DashboardIndexMessagesByCategoryComponentPartial : ViewComponent
    {
        private readonly EmailContext _context;
        private readonly UserManager<AppUser> _userManager;

        public _DashboardIndexMessagesByCategoryComponentPartial(
            EmailContext context,
            UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new DashboardMessagesByCategoryViewModel();

            var user = await _userManager.GetUserAsync(UserClaimsPrincipal);

            if (user == null || string.IsNullOrWhiteSpace(user.Email))
                return View(model);

            var userEmail = user.Email;

            var categoryData = await _context.Messages
                .AsNoTracking()
                .Where(x =>
                    (x.ReceiverEmail == userEmail || x.SenderEmail == userEmail) &&
                    !x.IsTrash &&
                    !x.IsDraft)
                .GroupBy(x => x.CategoryId)
                .Select(g => new
                {
                    CategoryId = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();

            var categoryIds = categoryData
                .Where(x => x.CategoryId.HasValue)
                .Select(x => x.CategoryId.Value)
                .ToList();

            var categories = await _context.Categories
                .AsNoTracking()
                .Where(x => categoryIds.Contains(x.CategoryId))
                .ToDictionaryAsync(x => x.CategoryId, x => x.CategoryName);

            var colors = new[]
            {
                "#8B7FD6",
                "#5CC09A",
                "#EBA27A",
                "#6FA8E8",
                "#F2B84B",
                "#C9CBD6",
                "#7DD3B0"
            };

            model.Items = categoryData
                .Select((item, index) => new MessagesByCategoryItemViewModel
                {
                    Label = item.CategoryId.HasValue &&
                            categories.TryGetValue(item.CategoryId.Value, out var categoryName)
                                ? categoryName
                                : "Kategorisiz",

                    Count = item.Count,
                    Color = colors[index % colors.Length]
                })
                .OrderByDescending(x => x.Count)
                .ToList();

            return View(model);
        }
    }
}