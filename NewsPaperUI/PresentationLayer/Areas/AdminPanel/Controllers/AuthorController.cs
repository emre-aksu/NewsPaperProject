using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Areas.AdminPanel.Controllers
{
    public class AuthorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
