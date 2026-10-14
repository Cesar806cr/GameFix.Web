using Gamefix.Application.DTOs;

namespace Gamefix.Application.Services.Interfaces;

public interface IServiceReserva
{
    Task<ICollection<ReservaDTO>> ListBySucursalAsync(int sucursalId);
    Task<ReservaDTO?> FindByIdAsync(int id, int sucursalId);
}
