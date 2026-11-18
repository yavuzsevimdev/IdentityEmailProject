using Microsoft.AspNetCore.Mvc;

namespace MYAcademy_IdentityEmailProject.ViewComponents.DashboardViewComponents
{
    public class _DashboardIndexHeaderComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
