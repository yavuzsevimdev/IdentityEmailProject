using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MYAcademy_IdentityEmailProject.Context;
using MYAcademy_IdentityEmailProject.Entities;
using MYAcademy_IdentityEmailProject.Models;
using System.ComponentModel.DataAnnotations;

namespace MYAcademy_IdentityEmailProject.Controllers
{
    [Authorize]
    public class MessageController : Controller
    {
        private readonly EmailContext _context;
        private readonly UserManager<AppUser> _userManager;

        public MessageController(EmailContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Inbox(string sortOrder = "newest")
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
                return Forbid();

            var userEmail = currentUser.Email;

            var messagesQuery = _context.Messages
                .Where(x => x.ReceiverEmail == userEmail && x.IsTrash == false);

            // Tarihe göre sıralama
            messagesQuery = sortOrder == "oldest" ? messagesQuery.OrderBy(x => x.SendDate) : messagesQuery.OrderByDescending(x => x.SendDate);

            // Mesajları getir
            var messages = await messagesQuery.ToListAsync();

            var categories = await _context.Categories
                .Select(c => new CategoryFilterViewModel
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName,
                    MessageCount = c.Messages.Count(m =>
                        m.ReceiverEmail == currentUser.Email)
                })
                .Where(c => c.MessageCount > 0)
                .ToListAsync();

            var model = new InboxViewModel
            {
                Messages = messages,
                Categories = categories,
                SortOrder = sortOrder
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult CreateMessage()
        {
            ViewBag.Categories = new SelectList(_context.Categories.ToList(),"CategoryId","CategoryName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMessage(Message message)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            
            if (currentUser == null)
                return Forbid();

            if (string.IsNullOrWhiteSpace(message.ReceiverEmail))
            {
                ModelState.AddModelError(nameof(message.ReceiverEmail),"Alıcı e-posta adresi zorunludur.");
            }
            else if (!new EmailAddressAttribute().IsValid(message.ReceiverEmail))
            {
                ModelState.AddModelError(nameof(message.ReceiverEmail),"Geçerli bir e-posta adresi giriniz.");
            }

            if (string.IsNullOrWhiteSpace(message.Subject))
            {
                ModelState.AddModelError(nameof(message.Subject),"Mesaj konusu zorunludur.");
            }
            else if (message.Subject.Length > 200)
            {
                ModelState.AddModelError(nameof(message.Subject),"Konu en fazla 200 karakter olabilir.");
            }

            if (string.IsNullOrWhiteSpace(message.MessageDetail) ||string.IsNullOrWhiteSpace(System.Text.RegularExpressions.Regex.Replace(message.MessageDetail,"<.*?>",string.Empty).Trim()))
            {
                ModelState.AddModelError(nameof(message.MessageDetail),"Mesaj içeriği boş bırakılamaz.");
            }

            var categoryExists = await _context.Categories.AnyAsync(x => x.CategoryId == message.CategoryId);

            if (!categoryExists)
            {
                ModelState.AddModelError(nameof(message.CategoryId),"Geçerli bir kategori seçiniz.");
            }

            message.SenderEmail = currentUser.Email;
            message.Category = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryId == message.CategoryId);
            message.SendDate = DateTime.Now;

            ModelState.Remove(nameof(message.SenderEmail));
            ModelState.Remove(nameof(message.Category));

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(),"CategoryId","CategoryName",message.CategoryId);
                return View("CreateMessage", message);
            }
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return RedirectToAction("SendBox");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> SendBox(string sortOrder = "newest")
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Forbid();

            var query = _context.Messages
                .AsNoTracking()
                .Where(x => x.SenderEmail == user.Email);

            query = sortOrder == "oldest"
                ? query.OrderBy(x => x.SendDate)
                : query.OrderByDescending(x => x.SendDate);

            var categories = await _context.Categories
                .Select(c => new CategoryFilterViewModel
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName,
                    MessageCount = c.Messages.Count(m =>
                        m.ReceiverEmail == user.Email)
                })
                .Where(c => c.MessageCount > 0)
                .ToListAsync();

            var model = new SendboxViewModel
            {
                Messages = await query.ToListAsync(),
                Categories = categories,
                 SortOrder = sortOrder == "oldest" ? "oldest" : "newest"
            };

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> MessageDetail(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
                return Forbid();

            var message = await _context.Messages.Include(x => x.Category).FirstOrDefaultAsync(x => x.MessageId == id && (x.ReceiverEmail == currentUser.Email || x.SenderEmail == currentUser.Email));

            if (message == null)
                return NotFound();

            if (!message.IsRead)
            {
                message.IsRead = true;
                await _context.SaveChangesAsync();
            }

            var sender = await _userManager.FindByEmailAsync(message.SenderEmail);
            var model = new MessageDetailViewModel
            {
                MessageId = message.MessageId,
                Subject = message.Subject,
                MessageDetail = message.MessageDetail,

                SenderEmail = message.SenderEmail,
                SenderName = sender != null ? $"{sender.Name} {sender.Surname}" : message.SenderEmail,
                SenderImageUrl = sender?.ImageUrl,

                ReceiverEmail = message.ReceiverEmail,
                ReceiverName = currentUser.Name + " " + currentUser.Surname,

                SendDate = message.SendDate,
                IsRead = message.IsRead,
                IsFavorite = message.IsFavorite,

                CategoryName = message.Category?.CategoryName ?? "Kategorisiz"
            };

            return View(model);
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> FavoriteMessages(string sortOrder = "newest")
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Forbid();

            var query = _context.Messages
                .AsNoTracking()
                .Where(x => x.ReceiverEmail == user.Email && x.IsFavorite);

            query = sortOrder == "oldest"
                ? query.OrderBy(x => x.SendDate)
                : query.OrderByDescending(x => x.SendDate);

            var messages = await query.ToListAsync();

            var categoryList = await _context.Categories
                .AsNoTracking()
                .ToListAsync();

            var categories = categoryList
                .Select(c => new CategoryFilterViewModel
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName,
                    MessageCount = messages.Count(x => x.CategoryId == c.CategoryId)
                })
                .Where(c => c.MessageCount > 0)
                .ToList();

            var model = new InboxViewModel
            {
                Messages = messages,
                Categories = categories,
                SortOrder = sortOrder
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleFavorite(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
                return Forbid();

            var message = await _context.Messages
                .FirstOrDefaultAsync(x =>
                    x.MessageId == id &&
                    x.ReceiverEmail == currentUser.Email);

            if (message == null)
                return NotFound();

            message.IsFavorite = !message.IsFavorite;

            await _context.SaveChangesAsync();

            var returnUrl = Request.Headers.Referer.ToString();

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction(nameof(Inbox));
        }


        [HttpGet]
        public async Task<IActionResult> Drafts(string sortOrder = "newest")
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
                return Forbid();

            var drafts = _context.Messages
                .Where(x => x.SenderEmail == currentUser.Email && x.IsDraft)
                .AsQueryable();

            drafts = sortOrder == "oldest"
                ? drafts.OrderBy(x => x.SendDate)
                : drafts.OrderByDescending(x => x.SendDate);

            var model = await drafts.ToListAsync();

            ViewBag.SortOrder = sortOrder;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveDraft(Message message)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
                return Forbid();

            Message draft;

            if (message.MessageId > 0)
            {
                // Mevcut taslağı bul
                draft = await _context.Messages.FirstOrDefaultAsync(x =>
                    x.MessageId == message.MessageId &&
                    x.SenderEmail == currentUser.Email &&
                    x.IsDraft);

                if (draft == null)
                    return NotFound();
            }
            else
            {
                // Yeni taslak oluştur
                draft = new Message
                {
                    SenderEmail = currentUser.Email,
                    IsDraft = true,
                    SendDate = DateTime.Now,
                    IsRead = false,
                    IsFavorite = false
                };

                _context.Messages.Add(draft);
            }

            // Alanları güncelle
            draft.ReceiverEmail = message.ReceiverEmail;
            draft.Subject = message.Subject;
            draft.MessageDetail = message.MessageDetail;
            draft.CategoryId = message.CategoryId;

            // Taslak kaydında zorunlu alan doğrulamalarını kaldır
            ModelState.Clear();

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Drafts));
        }



        [HttpGet]
        public async Task<IActionResult> EditDraft(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
                return Forbid();

            var draft = await _context.Messages
                .FirstOrDefaultAsync(x =>
                    x.MessageId == id &&
                    x.SenderEmail == currentUser.Email &&
                    x.IsDraft);

            if (draft == null)
                return NotFound();

            ViewBag.Categories = new SelectList(
                await _context.Categories.ToListAsync(),
                "CategoryId",
                "CategoryName",
                draft.CategoryId
            );


            return View("CreateMessage", draft);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateDraft(Message message)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
                return Forbid();

            var draft = await _context.Messages
                .FirstOrDefaultAsync(x =>
                    x.MessageId == message.MessageId &&
                    x.SenderEmail == currentUser.Email &&
                    x.IsDraft);

            if (draft == null)
                return NotFound();

            draft.ReceiverEmail = message.ReceiverEmail;
            draft.Subject = message.Subject;
            draft.MessageDetail = message.MessageDetail;
            draft.CategoryId = message.CategoryId;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Drafts));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendDraft(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
                return Forbid();

            var draft = await _context.Messages
                .FirstOrDefaultAsync(x =>
                    x.MessageId == id &&
                    x.SenderEmail == currentUser.Email &&
                    x.IsDraft);

            if (draft == null)
                return NotFound();

            if (string.IsNullOrWhiteSpace(draft.ReceiverEmail) ||
                !new EmailAddressAttribute().IsValid(draft.ReceiverEmail))
            {
                TempData["Error"] = "Geçerli bir alıcı e-posta adresi giriniz.";
                return RedirectToAction(nameof(EditDraft), new { id });
            }

            if (string.IsNullOrWhiteSpace(draft.Subject) ||
                string.IsNullOrWhiteSpace(draft.MessageDetail) ||
                draft.CategoryId == null)
            {
                TempData["Error"] = "Taslağın alıcı, konu, içerik ve kategori bilgilerini tamamlayınız.";
                return RedirectToAction(nameof(EditDraft), new { id });
            }

            draft.IsDraft = false;
            draft.SendDate = DateTime.Now;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(SendBox));
        }

        public async Task<IActionResult> Trashes(string sortOrder = "newest")
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
                return Forbid();

            var userEmail = currentUser.Email;

            var messagesQuery = _context.Messages
                .Where(x => x.ReceiverEmail == userEmail && x.IsTrash == true);

            // Tarihe göre sıralama
            messagesQuery = sortOrder == "oldest" ? messagesQuery.OrderBy(x => x.SendDate) : messagesQuery.OrderByDescending(x => x.SendDate);

            // Mesajları getir
            var messages = await messagesQuery.ToListAsync();

            var categoryList = await _context.Categories
                .AsNoTracking()
                .ToListAsync();

            var categories = categoryList
                .Select(c => new CategoryFilterViewModel
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName,
                    MessageCount = messages.Count(x => x.CategoryId == c.CategoryId)
                })
                .Where(c => c.MessageCount > 0)
                .ToList();

            var model = new InboxViewModel
            {
                Messages = messages,
                Categories = categories,
                SortOrder = sortOrder
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateTrash(List<int> ids)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
                return Forbid();

            var messages = await _context.Messages
                .Where(x => ids.Contains(x.MessageId) &&
                            x.ReceiverEmail == currentUser.Email)
                .ToListAsync();

            foreach (var message in messages)
            {
                message.IsTrash = !message.IsTrash;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Inbox));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMessage(List<int> ids)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
                return Forbid();

            var messages = await _context.Messages
                .Where(x => ids.Contains(x.MessageId) &&
                            x.ReceiverEmail == currentUser.Email)
                .ToListAsync();

            foreach (var message in messages)
            {
                 _context.Messages.Remove(message);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Inbox));
        }

        [HttpGet]
        public async Task<IActionResult> ReplyMessage(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
                return Forbid();

            var originalMessage = await _context.Messages
                .FirstOrDefaultAsync(x =>
                    x.MessageId == id &&
                    x.ReceiverEmail == currentUser.Email);

            if (originalMessage == null)
                return NotFound();

            var model = new Message
            {
                ReceiverEmail = originalMessage.SenderEmail,
                Subject = originalMessage.Subject.StartsWith("Re:")
                    ? originalMessage.Subject
                    : $"Re: {originalMessage.Subject}",
                MessageDetail = $@"

<br><br>
----- Orijinal Mesaj -----
<br>
Gönderen: {originalMessage.SenderEmail}
<br>
Tarih: {originalMessage.SendDate}
<br>
Konu: {originalMessage.Subject}
<br><br>
{originalMessage.MessageDetail}"
            };

            ViewBag.Categories = new SelectList(
                await _context.Categories.ToListAsync(),
                "CategoryId",
                "CategoryName");

            return View("CreateMessage", model);
        }

        [HttpGet]
        public async Task<IActionResult> ForwardMessage(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
                return Forbid();

            var originalMessage = await _context.Messages
                .FirstOrDefaultAsync(x =>
                    x.MessageId == id &&
                    x.ReceiverEmail == currentUser.Email);

            if (originalMessage == null)
                return NotFound();

            var model = new Message
            {
                ReceiverEmail = "",
                Subject = originalMessage.Subject.StartsWith("Fwd:")
                    ? originalMessage.Subject
                    : $"Fwd: {originalMessage.Subject}",
                MessageDetail = $@"

<br><br>
----- İletilen Mesaj -----
<br>
Gönderen: {originalMessage.SenderEmail}
<br>
Tarih: {originalMessage.SendDate}
<br>
Konu: {originalMessage.Subject}
<br><br>
{originalMessage.MessageDetail}"
            };

            ViewBag.Categories = new SelectList(
                await _context.Categories.ToListAsync(),
                "CategoryId",
                "CategoryName");

            return View("CreateMessage", model);
        }
    }
}
