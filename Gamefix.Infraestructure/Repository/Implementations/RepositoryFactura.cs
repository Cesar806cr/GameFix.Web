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

    public async Task<ICollection<Factura>> ListAsync(int sucursalId, int? clienteId = null, DateTime? fecha = null)
    {
        var query = Query()
            .Where(factura => factura.SucursalId == sucursalId);

        if (clienteId.HasValue)
            query = query.Where(factura => factura.ClienteId == clienteId.Value);

        if (fecha.HasValue)
        {
            var siguiente = fecha.Value.Date.AddDays(1);
            query = query.Where(factura =>
                factura.FechaEmision >= fecha.Value.Date &&
                factura.FechaEmision < siguiente);
        }

        return await query
            .OrderByDescending(factura => factura.FechaEmision)
            .ToListAsync();
    }

    public async Task<Factura?> FindByIdAsync(int id, int sucursalId)
    {
        return await Query()
            .FirstOrDefaultAsync(factura =>
                factura.FacturaId == id && factura.SucursalId == sucursalId);
    }

    public async Task<ICollection<Usuario>> ListClientesAsync() =>
        await _context.Usuario
            .Where(usuario => usuario.RolId == 3 && usuario.Activo)
            .OrderBy(usuario => usuario.Nombre)
            .AsNoTracking()
            .ToListAsync();

    public Task<Usuario?> FindUsuarioAsync(int usuarioId) =>
        _context.Usuario
            .AsNoTracking()
            .FirstOrDefaultAsync(usuario => usuario.UsuarioId == usuarioId && usuario.Activo);

    public Task<Sucursal?> FindSucursalAsync(int sucursalId) =>
        _context.Sucursal
            .AsNoTracking()
            .FirstOrDefaultAsync(sucursal => sucursal.SucursalId == sucursalId && sucursal.Activa);

    public async Task<ICollection<Servicio>> ListServiciosAsync() =>
        await _context.Servicio
            .Where(servicio => servicio.Activo)
            .OrderBy(servicio => servicio.Nombre)
            .AsNoTracking()
            .ToListAsync();

    public async Task<ICollection<Producto>> ListProductosAsync() =>
        await _context.Producto
            .Where(producto => producto.Activo && producto.Existencia > 0)
            .OrderBy(producto => producto.Nombre)
            .AsNoTracking()
            .ToListAsync();

    public async Task<string> GenerateNumeroAsync(bool esProforma, int sucursalId)
    {
        var prefijoNumeroFactura = esProforma ? "PRO-" : "FAC-";
        var numeros = await _context.Factura
            .Where(factura =>
                factura.SucursalId == sucursalId &&
                factura.NumeroFactura.StartsWith(prefijoNumeroFactura))
            .Select(factura => factura.NumeroFactura)
            .AsNoTracking()
            .ToListAsync();

        var siguiente = numeros
            .Select(numero => numero.Length > prefijoNumeroFactura.Length &&
                              int.TryParse(numero[prefijoNumeroFactura.Length..], out var valor)
                ? valor
                : 0)
            .DefaultIfEmpty(0)
            .Max() + 1;

        return $"{prefijoNumeroFactura}{siguiente:0000}";
    }

    public async Task AddAsync(Factura factura)
    {
        _context.Factura.Add(factura);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Factura factura)
    {
        _context.Factura.Attach(factura);
        _context.Entry(factura).Property(item => item.NumeroFactura).IsModified = true;
        _context.Entry(factura).Property(item => item.UsuarioRegistroId).IsModified = true;
        _context.Entry(factura).Property(item => item.FechaEmision).IsModified = true;
        _context.Entry(factura).Property(item => item.EsProforma).IsModified = true;
        _context.Entry(factura).Property(item => item.EstadoFactura).IsModified = true;
        _context.Entry(factura).Property(item => item.FechaPago).IsModified = true;
        await _context.SaveChangesAsync();
    }

    private IQueryable<Factura> Query()
    {
        return _context.Factura
            .Include(factura => factura.Cliente)
            .Include(factura => factura.Sucursal)
            .Include(factura => factura.UsuarioRegistro)
            .Include(factura => factura.DetalleFactura)
                .ThenInclude(detalle => detalle.Producto)
            .Include(factura => factura.DetalleFactura)
                .ThenInclude(detalle => detalle.Servicio)
            .AsNoTracking();
    }
}
