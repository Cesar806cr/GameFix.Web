using ReservasDeportivas.Domain.Entities;

namespace ReservasDeportivas.Infrastructure.Repositories;

public class ReservasRepository
{
    public List<Actividad> Actividades { get; } =
    [
        new Actividad
        {
            Id = 1,
            Nombre = "Fútbol 5",
            Descripcion = "Reserva una cancha para jugar fútbol.",
            DuracionMinutos = 120,
            Precio = 25000,
            ImagenUrl = "https://images.unsplash.com/photo-1522778119026-d647f0596c20",
            Activa = true
        },
        new Actividad
        {
            Id = 2,
            Nombre = "Tenis",
            Descripcion = "Disfruta una sesión en la cancha de tenis.",
            DuracionMinutos = 60,
            Precio = 10000,
            ImagenUrl = "https://images.unsplash.com/photo-1622279457486-62dcc4a431d6",
            Activa = true
        },
        new Actividad
        {
            Id = 3,
            Nombre = "Baloncesto",
            Descripcion = "Reserva el espacio para baloncesto.",
            DuracionMinutos = 90,
            Precio = 15000,
            ImagenUrl = "https://images.unsplash.com/photo-1546519638-68e109498ffc",
            Activa = true
        }
    ];

    public List<Reservacion> Reservaciones { get; } = [];
}
