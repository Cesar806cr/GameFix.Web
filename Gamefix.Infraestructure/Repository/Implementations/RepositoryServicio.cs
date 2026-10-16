using Gamefix.Infraestructure.Data;
using Gamefix.Infraestructure.Models;
using Gamefix.Infraestructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gamefix.Infraestructure.Repository.Implementations;

public class RepositoryServicio : IRepositoryServicio
{
    private readonly GameFixContext _context;

    public RepositoryServicio(GameFixContext context)
    {
        _context = context;
    }

    public async Task<ICollection<Servicio>> ListAsync()
    {
        return await _context.Servicio
            .Where(servicio => servicio.Activo)
            .OrderBy(servicio => servicio.Nombre)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Servicio?> FindByIdAsync(int id)
    {
        return await _context.Servicio
            .AsNoTracking()
            .FirstOrDefaultAsync(servicio => servicio.ServicioId == id && servicio.Activo);
    }
}
