using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MYAcademy_IdentityEmailProject.Controllers
{
    [AllowAnonymous]
    public class ErrorPagesController : Controller
    {
        public IActionResult Page401()
        {
            return View();
        }

        public IActionResult Page403()
        {
            return View();
        }

        public IActionResult Page404()
        {
            return View();
        }

        public IActionResult Page500()
        {
            return View();
        }
    }
}