using Gamefix.Application.DTOs;

namespace Gamefix.Application.Services.Interfaces;

public interface IServiceSucursal
{
    Task<ICollection<SucursalDTO>> ListAsync();
    Task<SucursalDTO?> FindByIdAsync(int id);
    Task<ICollection<UsuarioOpcionDTO>> ListEncargadosDisponiblesAsync(int? sucursalId);
    Task AddAsync(SucursalDTO dto);
    Task UpdateAsync(SucursalDTO dto);
}
