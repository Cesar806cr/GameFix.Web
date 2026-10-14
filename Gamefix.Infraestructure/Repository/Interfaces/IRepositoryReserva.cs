using Gamefix.Infraestructure.Models;

namespace Gamefix.Infraestructure.Repository.Interfaces;

public interface IRepositoryReserva
{
    Task<ICollection<Reserva>> ListBySucursalAsync(int sucursalId);
    Task<Reserva?> FindByIdAsync(int id, int sucursalId);
}
