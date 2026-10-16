using Gamefix.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;
using Gamefix.Web.Util;

namespace Gamefix.Web.Controllers
{
    public class ProductoController : Controller
    {
        private readonly IServiceProducto _serviceProducto;

        public ProductoController(IServiceProducto serviceProducto)
        {
            _serviceProducto = serviceProducto;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (TempData.ContainsKey("Mensaje"))
            {
                ViewBag.NotificationMessage = TempData["Mensaje"];
            }

            ViewData["Title"] = "Productos";
            ViewBag.Titulo = "Lista de productos";

            var collection = await _serviceProducto.ListAsync();

            return View(collection);
        }

        [HttpGet]
        public async Task<IActionResult> IndexAdmin(int? page)
        {
            var collection = await _serviceProducto.ListAsync();

            ViewData["Title"] = "Mantenimiento de productos";

            return View(collection.ToPagedList(page ?? 1, 5));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var producto = await _serviceProducto.FindByIdAsync(id);

            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

    }
}
