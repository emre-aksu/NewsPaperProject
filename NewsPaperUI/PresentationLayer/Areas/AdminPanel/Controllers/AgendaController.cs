using Microsoft.AspNetCore.Mvc;
using PresentationLayer.ApiServices;
using PresentationLayer.Areas.AdminPanel.APITypes;
using PresentationLayer.Areas.AdminPanel.Models;
using System.Threading.Tasks;

namespace PresentationLayer.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class AgendaController : Controller
    {
        private readonly IApiService _apiService;
        public AgendaController()
        {
            IApiService _apiService;
        }
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var viewModel = await _apiService.GetData<List<AgendaResponse>>("agendas",token: null);
            return View(viewModel);
        }

      
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var model = new AgendaViewModel();

            var authorsResponse = await _apiService.GetData<List<AuthorResponse>>("authors");
            var categoriesResponse = await _apiService.GetData<List<CategoryResponse>>("categories");

            if (authorsResponse?.Data != null)
            {
                model.AuthorList = authorsResponse.Data;
            }

            if (categoriesResponse?.Data != null)
            {
                model.CategoryList = categoriesResponse.Data;
            }

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Add(AgendaViewModel model)
        {
            if (ModelState.IsValid)
            {
                var jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                var response = await _apiService.PostData("agendas", jsonData, token: null);
                if (response)
                {
                    return RedirectToAction("List");
                }
            }
            return View(model);
        }
    }
}
