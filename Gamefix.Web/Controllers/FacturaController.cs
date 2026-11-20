using Gamefix.Application.Services.Interfaces;
using Gamefix.Application.DTOs;
using Gamefix.Web.Util;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;

namespace Gamefix.Web.Controllers;

public class FacturaController : Controller
{
    private const int SucursalEncargadoId = 4;
    private const int UsuarioEncargadoId = 15;
    private readonly IServiceFactura _serviceFactura;

    public FacturaController(IServiceFactura serviceFactura)
    {
        _serviceFactura = serviceFactura;
    }

    public async Task<IActionResult> Index(int? page, int? clienteId, DateTime? fecha)
    {
        if (TempData.ContainsKey("Mensaje"))
            ViewBag.NotificationMessage = TempData["Mensaje"];

        var facturas = await _serviceFactura.ListAsync(
            SucursalEncargadoId, clienteId, fecha);
        var formulario = await _serviceFactura.GetFormularioAsync(
            SucursalEncargadoId, UsuarioEncargadoId);
        ViewBag.Clientes = formulario.Clientes;
        ViewBag.ClienteId = clienteId;
        ViewBag.Fecha = fecha?.ToString("yyyy-MM-dd");
        return View(facturas.ToPagedList(page ?? 1, 5));
    }

    public async Task<IActionResult> Details(int id)
    {
        if (TempData.ContainsKey("Mensaje"))
            ViewBag.NotificationMessage = TempData["Mensaje"];

        var factura = await _serviceFactura.FindByIdAsync(id, SucursalEncargadoId);
        return factura == null ? NotFound() : View(factura);
    }

    [HttpGet]
    public async Task<IActionResult> Create(int? proformaId)
    {
        var formulario = await _serviceFactura.GetFormularioAsync(
            SucursalEncargadoId, UsuarioEncargadoId, proformaId);
        return View(formulario);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(FacturaFormularioDTO formulario)
    {
        if (!ModelState.IsValid)
        {
            var datos = await _serviceFactura.GetFormularioAsync(
                SucursalEncargadoId, UsuarioEncargadoId);
            formulario.SucursalNombre = datos.SucursalNombre;
            formulario.EncargadoNombre = datos.EncargadoNombre;
            formulario.Clientes = datos.Clientes;
            formulario.Servicios = datos.Servicios;
            formulario.Productos = datos.Productos;
            return View(formulario);
        }

        var resultado = await _serviceFactura.CreateAsync(
            formulario.Factura, SucursalEncargadoId, UsuarioEncargadoId);
        if (!resultado.Exito)
        {
            ModelState.AddModelError(string.Empty, resultado.Mensaje);
            var datos = await _serviceFactura.GetFormularioAsync(
                SucursalEncargadoId, UsuarioEncargadoId);
            formulario.SucursalNombre = datos.SucursalNombre;
            formulario.EncargadoNombre = datos.EncargadoNombre;
            formulario.Clientes = datos.Clientes;
            formulario.Servicios = datos.Servicios;
            formulario.Productos = datos.Productos;
            return View(formulario);
        }

        TempData["Mensaje"] = SweetAlertHelper.Mensaje(
            "Factura registrada", resultado.Mensaje, SweetAlertMessageType.success);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConvertirProforma(int id)
    {
        var resultado = await _serviceFactura.ConvertirProformaAsync(
            id, SucursalEncargadoId, UsuarioEncargadoId);
        TempData["Mensaje"] = SweetAlertHelper.Mensaje(
            resultado.Exito ? "Factura registrada" : "No se pudo registrar",
            resultado.Mensaje,
            resultado.Exito ? SweetAlertMessageType.success : SweetAlertMessageType.error);
        return RedirectToAction(nameof(Details), new { id });
    }
}
