using AutoMapper;
using Gamefix.Application.DTOs;
using Gamefix.Infraestructure.Models;

namespace Gamefix.Application.Profiles;

public class ServicioProfile : Profile
{
    public ServicioProfile()
    {
        CreateMap<Servicio, ServicioDTO>();
    }
}
