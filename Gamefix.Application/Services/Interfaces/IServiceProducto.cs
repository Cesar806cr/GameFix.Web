using Gamefix.Application.DTOs;
using Gamefix.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamefix.Application.Services.Interfaces
{
    public interface IServiceProducto
    {
        Task<ICollection<ProductoDTO>> ListAsync();

        Task<ProductoDTO?> FindByIdAsync(int id);
    }
}
