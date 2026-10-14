using Gamefix.Infraestructure.Data;
using Gamefix.Infraestructure.Models;
using Gamefix.Infraestructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gamefix.Infraestructure.Repository.Implementations;

public class RepositoryReserva : IRepositoryReserva
{
    private readonly GameFixContext _context;

    public RepositoryReserva(GameFixContext context)
    {
        _context = context;
    }

    public async Task<ICollection<Reserva>> ListBySucursalAsync(int sucursalId)
    {
        return await Query()
            .Where(reserva => reserva.SucursalId == sucursalId)
            .OrderByDescending(reserva => reserva.FechaHoraInicio)
            .ToListAsync();
    }

    public async Task<Reserva?> FindByIdAsync(int id, int sucursalId)
    {
        return await Query()
            .FirstOrDefaultAsync(reserva =>
                reserva.ReservaId == id && reserva.SucursalId == sucursalId);
    }

    private IQueryable<Reserva> Query()
    {
        return _context.Reserva
            .Include(reserva => reserva.Cliente)
            .Include(reserva => reserva.Sucursal)
            .Include(reserva => reserva.Servicio)
            .Include(reserva => reserva.EstadoReserva)
            .AsNoTracking();
    }
}
