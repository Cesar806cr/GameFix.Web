
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReservasDeportivas.Application.Interfaces;

namespace ReservasDeportivas.Web.Controllers;

public class AdminController : Controller
{
    private readonly IReservacionService _service;

    public AdminController(IReservacionService service)
    {
        _service = service;
    }

    private bool EsAdministrador()
    {
        return HttpContext.Session.GetString("Perfil")
            == "Administrador";
    }

    [HttpGet]
    public IActionResult Index()
    {
        if (!EsAdministrador())
        {
            return RedirectToAction("Index", "Reservas");
        }

        return View(_service.ObtenerActividades());
    }

    [HttpGet]
    public IActionResult Agenda(int? actividadId)
    {
        if (!EsAdministrador())
        {
            return StatusCode(StatusCodes.Status403Forbidden, new
            {
                mensaje = "No tienes permiso para consultar la agenda."
            });
        }

        var actividades = _service.ObtenerActividades()
            .ToDictionary(a => a.Id, a => a.Nombre);

        var reservas = _service.ObtenerReservaciones(actividadId);

        var eventos = reservas.Select(r =>
        {
            actividades.TryGetValue(r.ActividadId, out var nombreActividad);

            return new
            {
                id = r.Id.ToString(),
                title = $"{nombreActividad ?? "Actividad"} - {r.NombreCliente}",
                start = r.Inicio.ToString("yyyy-MM-ddTHH:mm:ss"),
                end = r.Fin.ToString("yyyy-MM-ddTHH:mm:ss"),
                extendedProps = new
                {
                    actividadId = r.ActividadId,
                    actividad = nombreActividad ?? "Actividad deportiva",
                    cliente = r.NombreCliente,
                    telefono = r.Telefono,
                    correo = r.Correo,
                    estado = r.Estado
                }
            };
        });

        return Json(eventos);
    }

    [HttpDelete]
    [ValidateAntiForgeryToken]
    public IActionResult EliminarActividad(int id)
    {
        if (!EsAdministrador())
        {
            return StatusCode(StatusCodes.Status403Forbidden, new
            {
                mensaje = "No tienes permiso para desactivar actividades."
            });
        }

        try
        {
            // Este método debe existir en IReservacionService
            // y en la implementación concreta del servicio.
            _service.DesactivarActividad(id);

            return Ok(new
            {
                mensaje = "La actividad fue desactivada correctamente."
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { mensaje = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }
}
