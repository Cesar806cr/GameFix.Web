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

        public async Task<Producto> FindByIdAsync(int id)
        {
            var producto = await _context.Set<Producto>().FindAsync(id);
            if (producto == null)
            {
                throw new KeyNotFoundException($"No se encontró un producto con el ID {id}.");
            }
            return producto;
        }

        public async Task<ICollection<Producto>> ListAsync()
        {
            var collection = await _context.Set<Producto>().ToListAsync();
            return collection;
        }
    }
}
