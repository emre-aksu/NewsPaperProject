using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Areas.AdminPanel.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
