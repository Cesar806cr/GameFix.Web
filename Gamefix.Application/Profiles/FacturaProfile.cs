using AutoMapper;
using Gamefix.Application.DTOs;
using Gamefix.Infraestructure.Models;

namespace Gamefix.Application.Profiles;

public class FacturaProfile : Profile
{
    public FacturaProfile()
    {
        CreateMap<Factura, FacturaDTO>()
            .ForMember(dest => dest.ClienteNombre,
                opt => opt.MapFrom(src => src.Cliente.Nombre))
            .ForMember(dest => dest.ClienteIdentificacion,
                opt => opt.MapFrom(src => src.Cliente.Identificacion))
            .ForMember(dest => dest.ClienteCorreo,
                opt => opt.MapFrom(src => src.Cliente.CorreoElectronico))
            .ForMember(dest => dest.SucursalNombre,
                opt => opt.MapFrom(src => src.Sucursal.Nombre))
            .ForMember(dest => dest.SucursalTelefono,
                opt => opt.MapFrom(src => src.Sucursal.Telefono))
            .ForMember(dest => dest.SucursalDireccion,
                opt => opt.MapFrom(src => src.Sucursal.DireccionExacta))
            .ForMember(dest => dest.SucursalCorreo,
                opt => opt.MapFrom(src => src.Sucursal.CorreoElectronico))
            .ForMember(dest => dest.Detalles,
                opt => opt.MapFrom(src => src.DetalleFactura));

        CreateMap<DetalleFactura, FacturaDetalleDTO>()
            .ForMember(dest => dest.Subtotal,
                opt => opt.MapFrom(src => src.Subtotal ?? src.Cantidad * src.PrecioUnitario));
    }
}
