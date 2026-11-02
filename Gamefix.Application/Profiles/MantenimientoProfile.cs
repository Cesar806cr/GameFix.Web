using AutoMapper;
using Gamefix.Application.DTOs;
using Gamefix.Infraestructure.Models;

namespace Gamefix.Application.Profiles;

public class MantenimientoProfile : Profile
{
    public MantenimientoProfile()
    {
        CreateMap<CategoriaProducto, CategoriaProductoDTO>();
        CreateMap<Sucursal, SucursalDTO>()
            .ForMember(dest => dest.EncargadoIds,
                opt => opt.MapFrom(src => src.Usuario
                    .Where(usuario => usuario.RolId == 2)
                    .Select(usuario => usuario.UsuarioId)))
            .ForMember(dest => dest.EncargadosAsignados,
                opt => opt.MapFrom(src => src.Usuario
                    .Where(usuario => usuario.RolId == 2)));
        CreateMap<SucursalDTO, Sucursal>()
            .ForMember(dest => dest.Usuario, opt => opt.Ignore())
            .ForMember(dest => dest.HorarioSucursal, opt => opt.Ignore())
            .ForMember(dest => dest.BloqueoHorario, opt => opt.Ignore())
            .ForMember(dest => dest.Reserva, opt => opt.Ignore())
            .ForMember(dest => dest.Factura, opt => opt.Ignore());
        CreateMap<Usuario, UsuarioOpcionDTO>();
        CreateMap<HorarioSucursal, HorarioSucursalDTO>();
        CreateMap<HorarioSucursalDTO, HorarioSucursal>()
            .ForMember(dest => dest.Sucursal, opt => opt.Ignore());
        CreateMap<BloqueoHorario, BloqueoHorarioDTO>();
        CreateMap<BloqueoHorarioDTO, BloqueoHorario>()
            .ForMember(dest => dest.Sucursal, opt => opt.Ignore())
            .ForMember(dest => dest.UsuarioRegistro, opt => opt.Ignore());
    }
}
