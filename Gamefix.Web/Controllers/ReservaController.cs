using Gamefix.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;

namespace Gamefix.Web.Controllers;

public class ReservaController : Controller
{
    private const int SucursalEncargadoId = 1;
    private readonly IServiceReserva _serviceReserva;

    public ReservaController(IServiceReserva serviceReserva)
    {
        _serviceReserva = serviceReserva;
    }

    public async Task<IActionResult> Index(int? page)
    {
        var reservas = await _serviceReserva.ListBySucursalAsync(SucursalEncargadoId);
        return View(reservas.ToPagedList(page ?? 1, 5));
    }

    public async Task<IActionResult> Details(int id)
    {
        var reserva = await _serviceReserva.FindByIdAsync(id, SucursalEncargadoId);
        return reserva == null ? NotFound() : View(reserva);
    }
}
