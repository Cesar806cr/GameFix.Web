using ReservasDeportivas.Application.DTOs;
using ReservasDeportivas.Domain.Entities;

namespace ReservasDeportivas.Application.Interfaces;

public interface IReservacionService
{
    IReadOnlyList<Actividad> ObtenerActividades();

    IReadOnlyList<string> ObtenerHorariosDisponibles(
        int actividadId,
        DateTime fecha);

    IReadOnlyList<Reservacion> ObtenerReservaciones(
        int? actividadId);

    (bool Exito, string Mensaje, Reservacion? Datos)
        Crear(ReservacionCrearDto dto);

    bool EliminarReservacion(int id);

    bool EliminarActividad(int id);

    void DesactivarActividad(int actividadId);
}
