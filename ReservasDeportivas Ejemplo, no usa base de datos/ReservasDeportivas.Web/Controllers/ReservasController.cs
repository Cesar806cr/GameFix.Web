using Microsoft.AspNetCore.Mvc;
using ReservasDeportivas.Application.DTOs;
using ReservasDeportivas.Application.Interfaces;

namespace ReservasDeportivas.Web.Controllers;

public class ReservasController : Controller
{
    private readonly IReservacionService _service;

    public ReservasController(IReservacionService service)
    {
        _service = service;
    }

    public IActionResult Index()
    {
        return View(_service.ObtenerActividades());
    }

    [HttpGet]
    public IActionResult Horarios(int actividadId, DateTime fecha)
    {
        return Json(_service.ObtenerHorariosDisponibles(
            actividadId, fecha));
    }

    [HttpGet]
    public IActionResult BuscarActividades(string termino)
    {
        if (string.IsNullOrWhiteSpace(termino))
            return Json(Array.Empty<object>());

        var resultados = _service.ObtenerActividades()
            .Where(a => a.Nombre.Contains(
                termino.Trim(),
                StringComparison.OrdinalIgnoreCase))
            .Select(a => new
            {
                id = a.Id,
                nombre = a.Nombre
            });

        return Json(resultados);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Crear([FromBody] ReservacionCrearDto dto)
    {
        if (!ModelState.IsValid)
        {
            var errores = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .SelectMany(x => x.Value!.Errors)
                .Select(x => x.ErrorMessage);

            return BadRequest(new
            {
                mensaje = "Revise los datos ingresados.",
                errores
            });
        }

        var resultado = _service.Crear(dto);

        if (!resultado.Exito)
        {
            return BadRequest(new
            {
                mensaje = resultado.Mensaje
            });
        }

        return Ok(new
        {
            mensaje = resultado.Mensaje,
            reservacion = resultado.Datos
        });
    }

    [HttpGet]
    public IActionResult Agenda(int? actividadId)
    {
        var reservas = _service.ObtenerReservaciones(actividadId);

        var actividades = _service.ObtenerActividades()
            .ToDictionary(a => a.Id, a => a.Nombre);

        var eventos = reservas.Select(r => new
        {
            id = r.Id,
            title = actividades.TryGetValue(
                r.ActividadId, out var nombre)
                    ? $"{nombre} - {r.NombreCliente}"
                    : r.NombreCliente,
            start = r.Inicio.ToString("yyyy-MM-ddTHH:mm:ss"),
            end = r.Fin.ToString("yyyy-MM-ddTHH:mm:ss"),
            extendedProps = new
            {
                actividadId = r.ActividadId,
                cliente = r.NombreCliente,
                telefono = r.Telefono,
                correo = r.Correo,
                estado = r.Estado
            }
        });

        return Json(eventos);
    }

    [HttpDelete]
    public IActionResult Eliminar(int id)
    {
        return _service.EliminarReservacion(id)
            ? Ok(new { mensaje = "Reservación eliminada." })
            : NotFound(new { mensaje = "No se encontró la reservación." });
    }

    [HttpDelete]
    public IActionResult EliminarActividad(int id)
    {
        bool eliminada = _service.EliminarActividad(id);

        return eliminada
            ? Ok(new { mensaje = "Actividad desactivada." })
            : BadRequest(new
            {
                mensaje = "No existe o tiene reservaciones activas."
            });
    }
}
