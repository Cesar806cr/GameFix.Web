using System.Globalization;
using ReservasDeportivas.Application.DTOs;
using ReservasDeportivas.Application.Interfaces;
using ReservasDeportivas.Domain.Entities;
using ReservasDeportivas.Infrastructure.Repositories;

namespace ReservasDeportivas.Application.Services;

public class ReservacionService : IReservacionService
{
    private readonly ReservasRepository _repo;

    private static readonly TimeSpan Apertura =
        new(8, 0, 0);

    private static readonly TimeSpan Cierre =
        new(18, 0, 0);

    public ReservacionService(ReservasRepository repo)
    {
        _repo = repo;
    }

    public IReadOnlyList<Actividad> ObtenerActividades()
    {
        return _repo.Actividades
            .Where(a => a.Activa)
            .ToList();
    }

    public IReadOnlyList<string> ObtenerHorariosDisponibles(
        int actividadId, DateTime fecha)
    {
        var actividad = _repo.Actividades.FirstOrDefault(
            a => a.Id == actividadId && a.Activa);

        if (actividad is null ||
            fecha.Date < DateTime.Today)
        {
            return [];
        }

        var horarios = new List<string>();

        for (var hora = Apertura;
             hora + TimeSpan.FromMinutes(
                 actividad.DuracionMinutos) <= Cierre;
             hora = hora.Add(TimeSpan.FromMinutes(60)))//se cambia la cantidad del tiempo
        {
            var inicio = fecha.Date.Add(hora);
            var fin = inicio.AddMinutes(
                actividad.DuracionMinutos);

            bool ocupado = _repo.Reservaciones.Any(r =>
                r.ActividadId == actividadId &&
                r.Estado != "Cancelada" &&
                r.Inicio < fin &&
                r.Fin > inicio);

            if (!ocupado)
            {
                horarios.Add(hora.ToString(@"hh\:mm"));
            }
        }

        return horarios;
    }

    public IReadOnlyList<Reservacion> ObtenerReservaciones(
        int? actividadId)
    {
        IEnumerable<Reservacion> consulta =
            _repo.Reservaciones;

        if (actividadId.HasValue)
        {
            consulta = consulta.Where(r =>
                r.ActividadId == actividadId.Value);
        }

        return consulta.OrderBy(r => r.Inicio).ToList();
    }

    public (bool Exito, string Mensaje, Reservacion? Datos)
        Crear(ReservacionCrearDto dto)
    {
        if (!DateTime.TryParseExact(
                dto.HoraInicio,
                "HH:mm",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var hora))
        {
            return (false, "La hora seleccionada no es válida.", null);
        }

        var actividad = _repo.Actividades.FirstOrDefault(
            a => a.Id == dto.ActividadId && a.Activa);

        if (actividad is null)
        {
            return (false, "La actividad no existe o está inactiva.", null);
        }

        var fecha = dto.FechaActividad!.Value.Date;
        var inicio = fecha.Add(hora.TimeOfDay);
        var fin = inicio.AddMinutes(actividad.DuracionMinutos);

        if (fecha < DateTime.Today)
        {
            return (false, "No se permiten fechas pasadas.", null);
        }

        if (inicio.TimeOfDay < Apertura ||
            fin.TimeOfDay > Cierre)
        {
            return (false, "La reservación está fuera del horario de atención.", null);
        }

        bool ocupado = _repo.Reservaciones.Any(r =>
            r.ActividadId == actividad.Id &&
            r.Estado != "Cancelada" &&
            r.Inicio < fin &&
            r.Fin > inicio);

        if (ocupado)
        {
            return (false,
                "El horario acaba de ser ocupado. Seleccione otro.",
                null);
        }

        var reservacion = new Reservacion
        {
            Id = _repo.Reservaciones.Count == 0
                ? 1
                : _repo.Reservaciones.Max(r => r.Id) + 1,
            ActividadId = actividad.Id,
            NombreCliente = dto.NombreCliente.Trim(),
            Correo = dto.Correo.Trim(),
            Telefono = dto.Telefono.Trim(),
            FechaSolicitud = DateTime.Now,
            Inicio = inicio,
            Fin = fin,
            Estado = "Confirmada"
        };

        _repo.Reservaciones.Add(reservacion);

        return (true, "Reservación creada correctamente.", reservacion);
    }

    public bool EliminarReservacion(int id)
    {
        var reservacion = _repo.Reservaciones.FirstOrDefault(
            r => r.Id == id);

        return reservacion is not null &&
               _repo.Reservaciones.Remove(reservacion);
    }

    public bool EliminarActividad(int id)
    {
        var actividad = _repo.Actividades.FirstOrDefault(
            a => a.Id == id);

        if (actividad is null)
            return false;

        // No eliminar actividades con reservaciones.
        if (_repo.Reservaciones.Any(
                r => r.ActividadId == id &&
                     r.Estado != "Cancelada"))
            return false;

        actividad.Activa = false;
        return true;
    }

    public void DesactivarActividad(int actividadId)
    {
        // 1. Buscar la actividad por su identificador.
        // 2. Si no existe, lanzar KeyNotFoundException.
        // 3. Establecer actividad.Activa = false.
        // 4. Guardar el cambio en el repositorio.
    }
}
