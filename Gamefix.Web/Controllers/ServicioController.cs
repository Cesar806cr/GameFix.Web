using Gamefix.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
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
        return View(await _serviceServicio.ListAsync());
    }

    public async Task<IActionResult> IndexAdmin(int? page)
    {
        var servicios = await _serviceServicio.ListAsync();
        ViewData["Title"] = "Mantenimiento de servicios";
        return View(servicios.ToPagedList(page ?? 1, 5));
    }

    public async Task<IActionResult> Details(int id)
    {
        var servicio = await _serviceServicio.FindByIdAsync(id);
        return servicio == null ? NotFound() : View(servicio);
    }

}
