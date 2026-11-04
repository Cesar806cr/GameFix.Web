using Gamefix.Application.Services.Interfaces;
using Gamefix.Web.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList.Extensions;

namespace Gamefix.Web.Controllers;

public class ServicioController : Controller
{
    private readonly IServiceServicio _serviceServicio;

    public ServicioController(IServiceServicio serviceServicio)
    {
        _serviceServicio = serviceServicio;
    }

    public async Task<IActionResult> Index()
    {
        if (TempData.ContainsKey("Mensaje"))
        {
            ViewBag.NotificationMessage = TempData["Mensaje"];
        }

        return View(await _serviceServicio.ListAsync());
    }

    public async Task<IActionResult> IndexAdmin(int? page)
    {
        if (TempData.ContainsKey("Mensaje"))
        {
            ViewBag.NotificationMessage = TempData["Mensaje"];
        }

        var servicios = await _serviceServicio.ListAsync();
        ViewData["Title"] = "Mantenimiento de servicios";
        return View(servicios.ToPagedList(page ?? 1, 5));
    }

    public async Task<IActionResult> Details(int id)
    {
        var servicio = await _serviceServicio.FindByIdAsync(id);
        return servicio == null ? NotFound() : View(servicio);
    }

    [HttpGet]
    public IActionResult Create() => View(new Gamefix.Application.DTOs.ServicioDTO
    {
        DuracionMinutos = 60,
        NivelDificultad = "Básico"
    });

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        Gamefix.Application.DTOs.ServicioDTO dto,
        IFormFile? imageFile)
    {
        await AssignImageAsync(dto, imageFile);

        if (dto.DuracionMinutos % 60 != 0)
            ModelState.AddModelError(nameof(dto.DuracionMinutos),
                "La duración debe ser un múltiplo de 60 minutos.");
        if (!ModelState.IsValid)
            return View(dto);

        await _serviceServicio.AddAsync(dto);
        TempData["Mensaje"] = SweetAlertHelper.Mensaje(
            "Servicio",
            "Servicio creado correctamente.",
            SweetAlertMessageType.success);
        return RedirectToAction(nameof(IndexAdmin));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var dto = await _serviceServicio.FindByIdAsync(id);
        return dto is null ? NotFound() : View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        Gamefix.Application.DTOs.ServicioDTO dto,
        IFormFile? imageFile)
    {
        var existing = await _serviceServicio.FindByIdAsync(dto.ServicioId);
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

        if (dto.DuracionMinutos % 60 != 0)
            ModelState.AddModelError(nameof(dto.DuracionMinutos),
                "La duración debe ser un múltiplo de 60 minutos.");
        if (!ModelState.IsValid)
            return View(dto);

        await _serviceServicio.UpdateAsync(dto);
        TempData["Mensaje"] = SweetAlertHelper.Mensaje(
            "Servicio",
            "Servicio actualizado correctamente.",
            SweetAlertMessageType.success);
        return RedirectToAction(nameof(IndexAdmin));
    }

    private async Task AssignImageAsync(
        Gamefix.Application.DTOs.ServicioDTO dto,
        IFormFile? imageFile)
    {
        if (imageFile is null || imageFile.Length == 0)
            return;

        const long maxImageSize = 5 * 1024 * 1024;
        var allowedContentTypes = new[] { "image/jpeg", "image/png", "image/webp" };

        if (imageFile.Length > maxImageSize)
        {
            ModelState.AddModelError("Imagen",
                "La imagen no puede superar los 5 MB.");
            return;
        }

        if (!allowedContentTypes.Contains(imageFile.ContentType,
                StringComparer.OrdinalIgnoreCase))
        {
            ModelState.AddModelError("Imagen",
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
