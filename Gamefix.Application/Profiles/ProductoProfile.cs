using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Gamefix.Application.DTOs;
using Gamefix.Infraestructure.Models;   

namespace Gamefix.Application.Profiles
{
    public class ProductoProfile : Profile
    {
        public ProductoProfile()
        {
            CreateMap<Producto, ProductoDTO>()
                .ForMember(dest => dest.CategoriaNombre,
                    opt => opt.MapFrom(src => src.CategoriaProducto == null
                        ? string.Empty
                        : src.CategoriaProducto.Nombre));

            CreateMap<ProductoDTO, Producto>()
                .ForMember(dest => dest.CategoriaProducto, opt => opt.Ignore());
        }
    }
}
