using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Areas.AdminPanel.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
