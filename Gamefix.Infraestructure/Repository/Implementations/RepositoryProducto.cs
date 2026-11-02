using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gamefix.Infraestructure.Data;
using Gamefix.Infraestructure.Models;
using Gamefix.Infraestructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gamefix.Infraestructure.Repository.Implementations
{
    public class RepositoryProducto : IRepositoryProducto
    {
        private readonly GameFixContext _context;

        public RepositoryProducto(GameFixContext context)
        {
            _context = context;
        }

        public async Task<Producto?> FindByIdAsync(int id)
        {
            return await _context.Set<Producto>()
                .Include(producto => producto.CategoriaProducto)
                .AsNoTracking()
                .FirstOrDefaultAsync(producto => producto.ProductoId == id);
        }

        public async Task<ICollection<CategoriaProducto>> ListCategoriasAsync()
        {
            return await _context.CategoriaProducto
                .Where(categoria => categoria.Activo)
                .OrderBy(categoria => categoria.Nombre)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddAsync(Producto entity)
        {
            _context.Producto.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Producto entity)
        {
            _context.Producto.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<Producto>> ListAsync()
        {
            return await _context.Set<Producto>()
                .Include(producto => producto.CategoriaProducto)
                .Where(producto => producto.Activo)
                .OrderBy(producto => producto.Nombre)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
