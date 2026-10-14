using Gamefix.Application.DTOs;

namespace Gamefix.Application.Services.Interfaces;

public interface IServiceFactura
{
    Task<ICollection<FacturaDTO>> ListAsync();
    Task<FacturaDTO?> FindByIdAsync(int id);
}
