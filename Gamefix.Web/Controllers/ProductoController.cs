using Gamefix.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
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

        // GET: /Producto
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

        // GET: /Producto/Details/1
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

        // GET: /Producto/Enviar
        [HttpGet]
        public IActionResult Enviar()
        {
            TempData["Mensaje"] = SweetAlertHelper.Mensaje(
                "Producto",
                "Mensaje enviado correctamente",
                SweetAlertMessageType.success);

            return RedirectToAction(nameof(Index));
        }
    }
}
