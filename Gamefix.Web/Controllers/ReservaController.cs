using Gamefix.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gamefix.Web.Controllers;

public class ReservaController : Controller
{
    private const int SucursalEncargadoId = 1;
    private readonly IServiceReserva _serviceReserva;

    public ReservaController(IServiceReserva serviceReserva)
    {
        _serviceReserva = serviceReserva;
    }

    public async Task<IActionResult> Index()
    {
        var reservas = await _serviceReserva.ListBySucursalAsync(SucursalEncargadoId);
        return View(reservas);
    }

    public async Task<IActionResult> Details(int id)
    {
        var reserva = await _serviceReserva.FindByIdAsync(id, SucursalEncargadoId);
        return reserva == null ? NotFound() : View(reserva);
    }
}
