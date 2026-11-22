using Gamefix.Application.Services.Interfaces;
using Gamefix.Application.DTOs;
using Gamefix.Web.Util;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;

namespace Gamefix.Web.Controllers;

public class ReservaController : Controller
{
    private const int SucursalEncargadoId = 1;
    private const int UsuarioEncargadoId = 2;


    private readonly IServiceReserva _serviceReserva;

    public ReservaController(IServiceReserva serviceReserva)
    {
        _serviceReserva = serviceReserva;
    }

    public async Task<IActionResult> Index(int? page, int? clienteId, DateTime? fecha)
    {
        if (TempData.ContainsKey("Mensaje"))
            ViewBag.NotificationMessage = TempData["Mensaje"];

        var reservas = await _serviceReserva.ListBySucursalAsync(
            SucursalEncargadoId, clienteId, fecha);
        var formulario = await _serviceReserva.GetFormularioAsync(SucursalEncargadoId);
        ViewBag.Clientes = formulario.Clientes;
        ViewBag.ClienteId = clienteId;
        ViewBag.Fecha = fecha?.ToString("yyyy-MM-dd");
        return View(reservas.ToPagedList(page ?? 1, 5));
    }

    public async Task<IActionResult> Details(int id)
    {
        var reserva = await _serviceReserva.FindByIdAsync(id, SucursalEncargadoId);
        return reserva == null ? NotFound() : View(reserva);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var formulario = await _serviceReserva.GetFormularioAsync(SucursalEncargadoId);
        return View(formulario);
    }

    [HttpGet]
    public async Task<IActionResult> HorasDisponibles(DateOnly fecha, int servicioId)
    {
        var horas = await _serviceReserva.GetHorasDisponiblesAsync(
            SucursalEncargadoId, fecha, servicioId);
        return Json(horas);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ReservaFormularioDTO formulario)
    {
        if (formulario.Reserva.Fecha < DateOnly.FromDateTime(DateTime.Today))
        {
            ModelState.AddModelError(
                "Reserva.Fecha",
                "La fecha de la reserva no puede ser anterior a la fecha actual.");
        }

        if (!ModelState.IsValid)
        {
            var datos = await _serviceReserva.GetFormularioAsync(SucursalEncargadoId);
            formulario.SucursalNombre = datos.SucursalNombre;
            formulario.Clientes = datos.Clientes;
            formulario.Servicios = datos.Servicios;
            return View(formulario);
        }

        var resultado = await _serviceReserva.CreateAsync(
            formulario.Reserva, SucursalEncargadoId, UsuarioEncargadoId);
        if (!resultado.Exito)
        {
            ModelState.AddModelError(string.Empty, resultado.Mensaje);
            var datos = await _serviceReserva.GetFormularioAsync(SucursalEncargadoId);
            formulario.SucursalNombre = datos.SucursalNombre;
            formulario.Clientes = datos.Clientes;
            formulario.Servicios = datos.Servicios;
            return View(formulario);
        }

        TempData["Mensaje"] = SweetAlertHelper.Mensaje(
            "Reserva registrada", resultado.Mensaje, SweetAlertMessageType.success);
        return RedirectToAction(nameof(Index));
    }
}
