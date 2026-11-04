using Gamefix.Infraestructure.Data;
using Gamefix.Infraestructure.Models;
using Gamefix.Infraestructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gamefix.Infraestructure.Repository.Implementations;

public class RepositorySucursal : IRepositorySucursal
{
    private readonly GameFixContext _context;

    public RepositorySucursal(GameFixContext context) => _context = context;

    public async Task<ICollection<Sucursal>> ListAsync() =>
        await _context.Sucursal
            .Include(sucursal => sucursal.Usuario)
            .OrderBy(sucursal => sucursal.Nombre)
            .AsNoTracking()
            .ToListAsync();

    public async Task<Sucursal?> FindByIdAsync(int id) =>
        await _context.Sucursal
            .Include(sucursal => sucursal.Usuario)
            .FirstOrDefaultAsync(sucursal => sucursal.SucursalId == id);

    public async Task<ICollection<Usuario>> ListEncargadosDisponiblesAsync(int? sucursalId) =>
        await _context.Usuario
            .Include(usuario => usuario.Rol)
            .Where(usuario => usuario.Activo &&
                              usuario.RolId == 2 &&
                              (usuario.SucursalId == null ||
                               (sucursalId.HasValue && usuario.SucursalId == sucursalId)))
            .OrderBy(usuario => usuario.Nombre)
            .AsNoTracking()
            .ToListAsync();

    public async Task AddAsync(Sucursal entity, IEnumerable<int> encargadoIds)
    {
        _context.Sucursal.Add(entity);
        await _context.SaveChangesAsync();
        await AssignEncargadosAsync(entity, encargadoIds);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Sucursal entity, IEnumerable<int> encargadoIds)
    {
        var current = await _context.Sucursal
            .Include(sucursal => sucursal.Usuario)
            .FirstAsync(sucursal => sucursal.SucursalId == entity.SucursalId);

        current.Nombre = entity.Nombre;
        current.Descripcion = entity.Descripcion;
        current.Telefono = entity.Telefono;
        current.DireccionExacta = entity.DireccionExacta;
        current.CorreoElectronico = entity.CorreoElectronico;
        current.Activa = entity.Activa;

        foreach (var usuario in current.Usuario.Where(usuario => usuario.RolId == 2))
            usuario.SucursalId = null;

        await AssignEncargadosAsync(current, encargadoIds);
        await _context.SaveChangesAsync();
    }

    private async Task AssignEncargadosAsync(Sucursal sucursal, IEnumerable<int> encargadoIds)
    {
        var ids = encargadoIds.Distinct().ToArray();
        if (ids.Length == 0)
            return;

        var usuarios = await _context.Usuario
            .Where(usuario => ids.Contains(usuario.UsuarioId) && usuario.RolId == 2 &&
                              (usuario.SucursalId == null || usuario.SucursalId == sucursal.SucursalId))
            .ToListAsync();

        foreach (var usuario in usuarios)
            usuario.SucursalId = sucursal.SucursalId;
    }
}
