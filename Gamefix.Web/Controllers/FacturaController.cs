using Gamefix.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gamefix.Web.Controllers;

public class FacturaController : Controller
{
    private readonly IServiceFactura _serviceFactura;

    public FacturaController(IServiceFactura serviceFactura)
    {
        _serviceFactura = serviceFactura;
    }

    public async Task<IActionResult> Index()
    {
        var facturas = await _serviceFactura.ListAsync();
        return View(facturas);
    }

    public async Task<IActionResult> Details(int id)
    {
        var factura = await _serviceFactura.FindByIdAsync(id);
        return factura == null ? NotFound() : View(factura);
    }
}
