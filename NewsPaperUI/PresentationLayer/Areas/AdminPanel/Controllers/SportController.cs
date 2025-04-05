using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Areas.AdminPanel.Controllers
{
    public class SportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
