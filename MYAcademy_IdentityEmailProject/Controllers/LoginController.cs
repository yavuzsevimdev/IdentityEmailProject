using MailKit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MYAcademy_IdentityEmailProject.Entities;
using MYAcademy_IdentityEmailProject.Models;
using MYAcademy_IdentityEmailProject.Services;

namespace MYAcademy_IdentityEmailProject.Controllers
{
    public class LoginController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;



        public LoginController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IConfiguration configuration, IEmailService emailService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
            _emailService = emailService;
        }

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(UserLoginViewModel userLoginViewModel)
        {
            var user = await _userManager.FindByNameAsync(userLoginViewModel.Username);

            if (user == null)
            {
                ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı.");
                return View(userLoginViewModel);
            }

            var passwordValid = await _userManager.CheckPasswordAsync(user, userLoginViewModel.Password);

            if (!passwordValid)
            {
                ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı.");
                return View(userLoginViewModel);
            }

            if (!user.EmailConfirmed)
            {
                return RedirectToAction("EmailConfirm", "Login", new { username = user.UserName });
            }

            await _signInManager.SignInAsync(user, isPersistent: false);

            return RedirectToAction("Inbox", "Message");
        }

        [HttpGet]
        public async Task<IActionResult> EmailConfirm(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            ViewBag.Email = user.Email;
            ViewBag.Username = username;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EmailConfirm(string username, int validCode)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                ModelState.AddModelError("", "Kullanıcı bulunamadı.");
                ViewBag.Username = username;
                return View();
            }

            if (user.EmailConfirmed)
            {
                return RedirectToAction("Inbox", "Message");
            }

            if (user.ValidCode != validCode)
            {
                ModelState.AddModelError("", "Doğrulama kodu hatalı.");
                ViewBag.Username = username;
                return View();
            }

            user.EmailConfirmed = true;
            user.ValidCode = null;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                ViewBag.Username = username;
                return View();
            }

            return RedirectToAction("Inbox", "Message");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("SignIn", "Login");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                ModelState.AddModelError("", "E-posta adresi zorunludur.");
                return View();
            }

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return RedirectToAction("ForgotPasswordConfirmation");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var encodedToken = Microsoft.AspNetCore.WebUtilities.WebEncoders.Base64UrlEncode(System.Text.Encoding.UTF8.GetBytes(token));

            var resetLink = Url.Action("ResetPassword", "Login", new { email = user.Email, token = encodedToken }, protocol: Request.Scheme);

            await SendPasswordResetEmailAsync(user.Email!, resetLink!);

            return RedirectToAction("ForgotPasswordConfirmation");
        }

        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            ViewBag.Email = email;
            ViewBag.Token = token;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(
            string email,
            string token,
            string password,
            string confirmPassword)
        {
            if (password != confirmPassword)
            {
                ModelState.AddModelError("", "Şifreler eşleşmiyor.");
                ViewBag.Email = email;
                ViewBag.Token = token;
                return View();
            }

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                ModelState.AddModelError("", "Kullanıcı bulunamadı.");
                ViewBag.Email = email;
                ViewBag.Token = token;
                return View();
            }

            try
            {
                var decodedToken = System.Text.Encoding.UTF8.GetString(
                    Microsoft.AspNetCore.WebUtilities.WebEncoders.Base64UrlDecode(token));

                var result = await _userManager.ResetPasswordAsync(
                    user, decodedToken, password);

                if (result.Succeeded)
                    return RedirectToAction("SignIn");

                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
            }
            catch (FormatException)
            {
                ModelState.AddModelError("", "Geçersiz sıfırlama bağlantısı.");
            }

            ViewBag.Email = email;
            ViewBag.Token = token;

            return View();
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        private async Task SendPasswordResetEmailAsync(string email, string resetLink)
        {
            var htmlMessage = $@"
        <div style='font-family:Arial,sans-serif;max-width:600px;margin:auto;padding:30px;'>
            <h2 style='color:#6366f1;'>Mailora</h2>
            <h3>Şifre Yenileme Talebi</h3>
            <p>Hesabınız için şifre yenileme talebinde bulunuldu.</p>
            <p>Aşağıdaki bağlantıya tıklayarak yeni şifrenizi belirleyebilirsiniz.</p>
            <a href='{System.Net.WebUtility.HtmlEncode(resetLink)}'
               style='display:inline-block;background:#6366f1;color:white;
               padding:14px 24px;text-decoration:none;border-radius:8px;'>
               Şifremi Yenile
            </a>
            <p style='margin-top:25px;color:#777;'>
                Bu talebi siz oluşturmadıysanız e-postayı dikkate almayabilirsiniz.
            </p>
        </div>";

            var textMessage = $"Şifrenizi yenilemek için bağlantıyı açın: {resetLink}";

            await _emailService.SendHtmlEmailAsync(
                email,
                "Mailora - Şifre Yenileme",
                htmlMessage,
                textMessage);
        }
    }
}
