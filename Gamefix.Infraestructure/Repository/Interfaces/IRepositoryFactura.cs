using Gamefix.Infraestructure.Models;

namespace Gamefix.Infraestructure.Repository.Interfaces;

public interface IRepositoryFactura
{
    Task<ICollection<Factura>> ListAsync(int sucursalId, int? clienteId = null, DateTime? fecha = null);
    Task<Factura?> FindByIdAsync(int id, int sucursalId);
    Task<ICollection<Usuario>> ListClientesAsync();
    Task<Usuario?> FindUsuarioAsync(int usuarioId);
    Task<Sucursal?> FindSucursalAsync(int sucursalId);
    Task<ICollection<Servicio>> ListServiciosAsync();
    Task<ICollection<Producto>> ListProductosAsync();
    Task<string> GenerateNumeroAsync(bool esProforma);
    Task AddAsync(Factura factura);
    Task UpdateAsync(Factura factura);
}
