using Gamefix.Infraestructure.Models;

namespace Gamefix.Infraestructure.Repository.Interfaces;

public interface IRepositoryReserva
{
    Task<ICollection<Reserva>> ListBySucursalAsync(int sucursalId, int? clienteId = null, DateTime? fecha = null);
    Task<Reserva?> FindByIdAsync(int id, int sucursalId);
    Task<ICollection<Usuario>> ListClientesAsync();
    Task<ICollection<Servicio>> ListServiciosAsync();
    Task<Sucursal?> FindSucursalAsync(int sucursalId);
    Task<bool> HorarioDisponibleAsync(int sucursalId, DateTime inicio, DateTime fin);
    Task AddAsync(Reserva reserva, Factura proforma);
}
