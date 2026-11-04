using System.Diagnostics;
using Gamefix.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Gamefix.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
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

        [HttpGet]
        public IActionResult ErrorHandler(string? messagesJson)
        {
            var messages = string.IsNullOrWhiteSpace(messagesJson)
                ? new List<string> { "Ocurrió un error inesperado." }
                : JsonConvert.DeserializeObject<ErrorMiddlewareViewModel>(messagesJson)
                    ?.ListMessages ?? new List<string> { "Ocurrió un error inesperado." };

            return View(messages);
        }
    }
}
