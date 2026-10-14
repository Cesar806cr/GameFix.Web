using Gamefix.Infraestructure.Data;
using Gamefix.Infraestructure.Models;
using Gamefix.Infraestructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gamefix.Infraestructure.Repository.Implementations;

public class RepositoryFactura : IRepositoryFactura
{
    private readonly GameFixContext _context;

    public RepositoryFactura(GameFixContext context)
    {
        _context = context;
    }

    public async Task<ICollection<Factura>> ListAsync()
    {
        return await Query()
            .OrderByDescending(factura => factura.FechaEmision)
            .ToListAsync();
    }

    public async Task<Factura?> FindByIdAsync(int id)
    {
        return await Query()
            .FirstOrDefaultAsync(factura => factura.FacturaId == id);
    }

    private IQueryable<Factura> Query()
    {
        return _context.Factura
            .Include(factura => factura.Cliente)
            .Include(factura => factura.Sucursal)
            .Include(factura => factura.DetalleFactura)
                .ThenInclude(detalle => detalle.Producto)
            .Include(factura => factura.DetalleFactura)
                .ThenInclude(detalle => detalle.Servicio)
            .AsNoTracking();
    }
}
