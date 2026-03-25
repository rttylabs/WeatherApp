using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WeatherApp.Interfaces;
using WeatherApp.Models;

namespace WeatherApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWeatherService _weatherService;

        public HomeController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }
        public async Task<IActionResult> Index()
        {
            var viewModel = await _weatherService.GetWeatherAsync();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Refresh()
        {
            var viewModel = await _weatherService.GetWeatherAsync();
            return Json(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
