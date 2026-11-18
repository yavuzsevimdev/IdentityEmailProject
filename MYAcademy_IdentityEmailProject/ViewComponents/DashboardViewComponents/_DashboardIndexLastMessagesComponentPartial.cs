using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MYAcademy_IdentityEmailProject.Context;
using MYAcademy_IdentityEmailProject.Entities;
using MYAcademy_IdentityEmailProject.Models;

namespace MYAcademy_IdentityEmailProject.ViewComponents.DashboardViewComponents
{
    public class _DashboardIndexLastMessagesComponentPartial : ViewComponent
    {
        private readonly EmailContext _context;
        private readonly UserManager<AppUser> _userManager;

        public _DashboardIndexLastMessagesComponentPartial(
            EmailContext context,
            UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new DashboardLastMessagesViewModel();

            var user = await _userManager.GetUserAsync(UserClaimsPrincipal);

            if (user == null || string.IsNullOrEmpty(user.Email))
                return View(model);

            var userEmail = user.Email;

            // Kullanıcının gelen ve gönderilen son 6 mesajı
            var messages = await _context.Messages
                .AsNoTracking()
                .Where(x =>
                    (x.ReceiverEmail == userEmail || x.SenderEmail == userEmail) &&
                    !x.IsTrash &&
                    !x.IsDraft)
                .OrderByDescending(x => x.SendDate)
                .Take(6)
                .ToListAsync();

            // Mesajların karşı tarafındaki kullanıcıların e-posta adresleri
            var otherEmails = messages
                .Select(x => x.SenderEmail == userEmail
                    ? x.ReceiverEmail
                    : x.SenderEmail)
                .Distinct()
                .ToList();

            // Karşı tarafın kullanıcı bilgileri
            var users = await _userManager.Users
                .AsNoTracking()
                .Where(x => otherEmails.Contains(x.Email))
                .ToDictionaryAsync(x => x.Email!, x => x);

            var avatarClasses = new[]
            {
                "av-lav",
                "av-blue",
                "av-peach",
                "av-mint",
                "av-rose",
                "av-sun"
            };

            for (int i = 0; i < messages.Count; i++)
            {
                var message = messages[i];

                var isIncoming = message.ReceiverEmail == userEmail;

                var otherEmail = isIncoming
                    ? message.SenderEmail
                    : message.ReceiverEmail;

                users.TryGetValue(otherEmail, out var otherUser);

                var displayName = otherUser != null
                    ? $"{otherUser.Name} {otherUser.Surname}".Trim()
                    : otherEmail;

                if (string.IsNullOrWhiteSpace(displayName))
                    displayName = otherEmail;

                var initials = string.Concat(
                    displayName
                        .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                        .Take(2)
                        .Select(x => x[0].ToString().ToUpper())
                );

                model.RecentMessages.Add(new DashboardLastMessageItemViewModel
                {
                    MessageId = message.MessageId,
                    DisplayName = displayName,
                    Email = otherEmail,
                    Initials = initials,
                    ImageUrl = otherUser?.ImageUrl,
                    Subject = message.Subject,
                    SendDate = message.SendDate,
                    IsIncoming = isIncoming,
                    IsRead = message.IsRead,
                    IsFavorite = message.IsFavorite,
                    AvatarClass = avatarClasses[i % avatarClasses.Length]
                });
            }

            return View(model);
        }
    }
}