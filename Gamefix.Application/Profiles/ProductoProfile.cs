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
            CreateMap<ProductoDTO, Producto>().ReverseMap(); 
        }
    }
}
