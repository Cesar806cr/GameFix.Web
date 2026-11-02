using Gamefix.Application.DTOs;
using Gamefix.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gamefix.Web.Controllers;

public class AgendaController : Controller
{
    private readonly IServiceAgenda _service;

    public AgendaController(IServiceAgenda service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> Index(int? sucursalId)
    {
        var sucursales = await _service.ListSucursalesAsync();
        var selected = sucursalId ?? sucursales.FirstOrDefault()?.SucursalId ?? 0;
        var agenda = selected == 0 ? new AgendaSucursalDTO() :
            await _service.GetAgendaAsync(selected) ?? new AgendaSucursalDTO();
        ViewBag.Sucursales = sucursales;
        return View(agenda);
    }

    [HttpGet]
    public async Task<IActionResult> DetailsHorario(int id)
    {
        var horario = await _service.FindHorarioAsync(id);
        return horario is null ? NotFound() : View(horario);
    }

    [HttpGet]
    public async Task<IActionResult> DetailsBloqueo(int id)
    {
        var bloqueo = await _service.FindBloqueoAsync(id);
        return bloqueo is null ? NotFound() : View(bloqueo);
    }

    [HttpGet]
    public async Task<IActionResult> CreateHorario(int sucursalId)
    {
        ViewBag.Sucursales = await _service.ListSucursalesAsync();
        return View(new HorarioSucursalDTO { SucursalId = sucursalId, DiaSemana = 1 });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateHorario(HorarioSucursalDTO dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Sucursales = await _service.ListSucursalesAsync();
            return View(dto);
        }

        var result = await _service.AddHorarioAsync(dto);
        if (!result.Exito)
        {
            ModelState.AddModelError(string.Empty, result.Mensaje);
            ViewBag.Sucursales = await _service.ListSucursalesAsync();
            return View(dto);
        }

        TempData["Mensaje"] = result.Mensaje;
        return RedirectToAction(nameof(Index), new { sucursalId = dto.SucursalId });
    }

    [HttpGet]
    public async Task<IActionResult> CreateBloqueo(int sucursalId)
    {
        ViewBag.Sucursales = await _service.ListSucursalesAsync();
        return View(new BloqueoHorarioDTO
        {
            SucursalId = sucursalId,
            FechaInicio = DateOnly.FromDateTime(DateTime.Today),
            FechaFin = DateOnly.FromDateTime(DateTime.Today)
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateBloqueo(BloqueoHorarioDTO dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Sucursales = await _service.ListSucursalesAsync();
            return View(dto);
        }

        var result = await _service.AddBloqueoAsync(dto);
        if (!result.Exito)
        {
            ModelState.AddModelError(string.Empty, result.Mensaje);
            ViewBag.Sucursales = await _service.ListSucursalesAsync();
            return View(dto);
        }

        TempData["Mensaje"] = result.Mensaje;
        return RedirectToAction(nameof(Index), new { sucursalId = dto.SucursalId });
    }
}
