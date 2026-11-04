using Gamefix.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;
using Gamefix.Web.Util;
using Microsoft.AspNetCore.Http;

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
            if (TempData.ContainsKey("Mensaje"))
            {
                ViewBag.NotificationMessage = TempData["Mensaje"];
            }

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

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categorias = await _serviceProducto.ListCategoriasAsync();
            return View(new Gamefix.Application.DTOs.ProductoDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            Gamefix.Application.DTOs.ProductoDTO dto,
            IFormFile? imageFile)
        {
            await AssignImageAsync(dto, imageFile);

            if (!ModelState.IsValid)
            {
                ViewBag.Categorias = await _serviceProducto.ListCategoriasAsync();
                return View(dto);
            }

            await _serviceProducto.AddAsync(dto);
            TempData["Mensaje"] = SweetAlertHelper.Mensaje(
                "Producto",
                "Producto creado correctamente.",
                SweetAlertMessageType.success);
            return RedirectToAction(nameof(IndexAdmin));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _serviceProducto.FindByIdAsync(id);
            if (dto is null)
                return NotFound();

            ViewBag.Categorias = await _serviceProducto.ListCategoriasAsync();
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            Gamefix.Application.DTOs.ProductoDTO dto,
            IFormFile? imageFile)
        {
            var existing = await _serviceProducto.FindByIdAsync(dto.ProductoId);
            if (existing is null)
                return NotFound();

            if (imageFile is null || imageFile.Length == 0)
            {
                dto.Imagen = existing.Imagen;
                dto.NombreImagen = existing.NombreImagen;
                dto.TipoImagen = existing.TipoImagen;
            }
            else
            {
                await AssignImageAsync(dto, imageFile);
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Categorias = await _serviceProducto.ListCategoriasAsync();
                return View(dto);
            }

            await _serviceProducto.UpdateAsync(dto);
            TempData["Mensaje"] = SweetAlertHelper.Mensaje(
                "Producto",
                "Producto actualizado correctamente.",
                SweetAlertMessageType.success);
            return RedirectToAction(nameof(IndexAdmin));
        }

        private async Task AssignImageAsync(
            Gamefix.Application.DTOs.ProductoDTO dto,
            IFormFile? imageFile)
        {
            if (imageFile is null || imageFile.Length == 0)
                return;

            const long maxImageSize = 5 * 1024 * 1024;
            var allowedContentTypes = new[] { "image/jpeg", "image/png", "image/webp" };

            if (imageFile.Length > maxImageSize)
            {
                ModelState.AddModelError(string.Empty,
                    "La imagen no puede superar los 5 MB.");
                return;
            }

            if (!allowedContentTypes.Contains(imageFile.ContentType,
                    StringComparer.OrdinalIgnoreCase))
            {
                ModelState.AddModelError(string.Empty,
                    "Seleccione una imagen JPG, PNG o WEBP.");
                return;
            }

            await using var stream = new MemoryStream();
            await imageFile.CopyToAsync(stream);
            dto.Imagen = stream.ToArray();
            dto.NombreImagen = Path.GetFileName(imageFile.FileName);
            dto.TipoImagen = imageFile.ContentType;
        }

    }
}
