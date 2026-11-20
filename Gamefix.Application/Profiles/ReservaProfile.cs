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
            .ForMember(dest => dest.ClienteCorreo,
                opt => opt.MapFrom(src => src.Cliente.CorreoElectronico))
            .ForMember(dest => dest.ClienteId,
                opt => opt.MapFrom(src => src.ClienteId))
            .ForMember(dest => dest.ServicioId,
                opt => opt.MapFrom(src => src.ServicioId))
            .ForMember(dest => dest.ServicioPrecio,
                opt => opt.MapFrom(src => src.Servicio.Precio))
            .ForMember(dest => dest.DuracionMinutos,
                opt => opt.MapFrom(src => src.Servicio.DuracionMinutos))
            .ForMember(dest => dest.EstadoNombre,
                opt => opt.MapFrom(src => src.EstadoReserva.Nombre))
            .ForMember(dest => dest.EstadoColor,
                opt => opt.MapFrom(src => src.EstadoReserva.ColorHexadecimal));
    }
}
