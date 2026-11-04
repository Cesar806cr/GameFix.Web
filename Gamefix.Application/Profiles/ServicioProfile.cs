using AutoMapper;
using Gamefix.Application.DTOs;
using Gamefix.Infraestructure.Models;

namespace Gamefix.Application.Profiles;

public class ServicioProfile : Profile
{
    public ServicioProfile()
    {
        CreateMap<Servicio, ServicioDTO>();
        CreateMap<ServicioDTO, Servicio>()
            .ForMember(dest => dest.DetalleFactura, opt => opt.Ignore())
            .ForMember(dest => dest.Reserva, opt => opt.Ignore());
    }
}
