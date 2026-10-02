using AutoMapper;
using Gamefix.Application.DTOs;
using Gamefix.Application.Services.Interfaces;
using Gamefix.Infraestructure.Repository.Interfaces;
using Gamefix.Infraestructure.Repository.Implementations;
using Gamefix.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamefix.Application.Services.Implementations
{
    public class ServiceProducto : IServiceProducto
    {
        private readonly IRepositoryProducto _repository;
        private readonly IMapper _mapper;

        public ServiceProducto(
            IRepositoryProducto repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ProductoDTO?> FindByIdAsync(int id)
        {
            var producto = await _repository.FindByIdAsync(id);

            if (producto == null)
            {
                return null;
            }

            var productoMapeado = _mapper.Map<ProductoDTO>(producto);

            return productoMapeado;
        }

        public async Task<ICollection<ProductoDTO>> ListAsync()
        {
            var lista = await _repository.ListAsync();

            var coleccion = _mapper.Map<ICollection<ProductoDTO>>(lista);

            return coleccion;
        }
    }
}
