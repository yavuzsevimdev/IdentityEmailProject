using Microsoft.AspNetCore.Mvc;

namespace MYAcademy_IdentityEmailProject.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
