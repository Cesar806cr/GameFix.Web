using Gamefix.Infraestructure.Models;

namespace Gamefix.Infraestructure.Repository.Interfaces;

public interface IRepositoryServicio
{
    Task<ICollection<Servicio>> ListAsync();
    Task<Servicio?> FindByIdAsync(int id);
}
