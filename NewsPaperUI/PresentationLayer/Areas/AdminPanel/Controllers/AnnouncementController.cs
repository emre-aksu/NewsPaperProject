using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Areas.AdminPanel.Controllers
{
    public class AnnouncementController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
