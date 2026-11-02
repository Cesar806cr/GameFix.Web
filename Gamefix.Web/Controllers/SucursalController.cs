using Gamefix.Application.DTOs;
using Gamefix.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gamefix.Web.Controllers;

public class SucursalController : Controller
{
    private readonly IServiceSucursal _service;

    public SucursalController(IServiceSucursal service) => _service = service;

    public async Task<IActionResult> Index() => View(await _service.ListAsync());

    public async Task<IActionResult> Details(int id)
    {
        var dto = await _service.FindByIdAsync(id);
        return dto is null ? NotFound() : View(dto);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewBag.Encargados = await _service.ListEncargadosDisponiblesAsync(null);
        return View(new SucursalDTO());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SucursalDTO dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Encargados = await _service.ListEncargadosDisponiblesAsync(null);
            return View(dto);
        }

        await _service.AddAsync(dto);
        TempData["Mensaje"] = "Sucursal creada correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var dto = await _service.FindByIdAsync(id);
        if (dto is null)
            return NotFound();

        ViewBag.Encargados = await _service.ListEncargadosDisponiblesAsync(id);
        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(SucursalDTO dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Encargados = await _service.ListEncargadosDisponiblesAsync(dto.SucursalId);
            return View(dto);
        }

        await _service.UpdateAsync(dto);
        TempData["Mensaje"] = "Sucursal actualizada correctamente.";
        return RedirectToAction(nameof(Index));
    }
}
