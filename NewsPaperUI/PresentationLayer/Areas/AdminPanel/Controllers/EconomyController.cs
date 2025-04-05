using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Areas.AdminPanel.Controllers
{
    public class EconomyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
