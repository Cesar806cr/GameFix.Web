using Gamefix.Application.DTOs;

namespace Gamefix.Application.Services.Interfaces;

public interface IServiceServicio
{
    Task<ICollection<ServicioDTO>> ListAsync();
    Task<ServicioDTO?> FindByIdAsync(int id);
    Task AddAsync(ServicioDTO dto);
    Task UpdateAsync(ServicioDTO dto);
}
