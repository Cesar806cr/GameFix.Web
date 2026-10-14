using Gamefix.Infraestructure.Models;

namespace Gamefix.Infraestructure.Repository.Interfaces;

public interface IRepositoryFactura
{
    Task<ICollection<Factura>> ListAsync();
    Task<Factura?> FindByIdAsync(int id);
}
