using Gamefix.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;

namespace Gamefix.Web.Controllers;

public class ReservaController : Controller
{
    private const int SucursalEncargadoId = 1;


    // Esto hace que solo se muestren las reservas de la sucursal con ID 1, que es la sucursal del encargado logueado.


    //Un encargado solo debe verlas citas de la sucursal a la que pertenece.


    //Eso significa que el historial de reservas debe comportarse como el sistema de un encargado de sucursal, no como un listado donde el usuario pueda escoger cualquier sucursal.

    //1. El encargado debe establecerse en este controlador.
    //La aplicación debe saber qué encargado está utilizando el sistema y qué sucursal tiene asignada. Esa información debe estar definida en el código del controlador o venir de una sesión/autenticación segura.

    //No debe recibirse desde la URl.

    //significa que el encargado simulado pertenece a la sucursal con ID 1.

    //Por lo tanto, actualmente se está usando: como la sucursal del encargado.

    //2. El encargado solo debe ver las citas de su sucursal

    //Si el encargado pertenece a Alajuela, solo debería ver reservas de Alajuela yviceversa.

    //No debería ver reservas de heredia y san jose.



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
