using Gamefix.Infraestructure.Models;

namespace Gamefix.Infraestructure.Repository.Interfaces;

public interface IRepositorySucursal
{
    Task<ICollection<Sucursal>> ListAsync();
    Task<Sucursal?> FindByIdAsync(int id);
    Task<ICollection<Usuario>> ListEncargadosDisponiblesAsync(int? sucursalId);
    Task AddAsync(Sucursal entity, IEnumerable<int> encargadoIds);
    Task UpdateAsync(Sucursal entity, IEnumerable<int> encargadoIds);
}
