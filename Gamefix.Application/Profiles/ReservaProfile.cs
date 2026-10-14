using AutoMapper;
using Gamefix.Application.DTOs;
using Gamefix.Infraestructure.Models;

namespace Gamefix.Application.Profiles;

public class ReservaProfile : Profile
{
    public ReservaProfile()
    {
        CreateMap<Reserva, ReservaDTO>()
            .ForMember(dest => dest.SucursalNombre,
                opt => opt.MapFrom(src => src.Sucursal.Nombre))
            .ForMember(dest => dest.ServicioNombre,
                opt => opt.MapFrom(src => src.Servicio.Nombre))
            .ForMember(dest => dest.ClienteNombre,
                opt => opt.MapFrom(src => src.Cliente.Nombre))
            .ForMember(dest => dest.EstadoNombre,
                opt => opt.MapFrom(src => src.EstadoReserva.Nombre))
            .ForMember(dest => dest.EstadoColor,
                opt => opt.MapFrom(src => src.EstadoReserva.ColorHexadecimal));
    }
}
