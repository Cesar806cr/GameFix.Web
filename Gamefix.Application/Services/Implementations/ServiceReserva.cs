using AutoMapper;
using Gamefix.Application.DTOs;
using Gamefix.Application.Services.Interfaces;
using Gamefix.Infraestructure.Models;
using Gamefix.Infraestructure.Repository.Interfaces;

namespace Gamefix.Application.Services.Implementations;

public class ServiceReserva : IServiceReserva
{
    private readonly IRepositoryReserva _repository;
    private readonly IMapper _mapper;

    public ServiceReserva(IRepositoryReserva repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ICollection<ReservaDTO>> ListBySucursalAsync(int sucursalId, int? clienteId = null, DateTime? fecha = null)
    {
        return _mapper.Map<ICollection<ReservaDTO>>(
            await _repository.ListBySucursalAsync(sucursalId, clienteId, fecha));
    }

    public async Task<ReservaDTO?> FindByIdAsync(int id, int sucursalId)
    {
        var reserva = await _repository.FindByIdAsync(id, sucursalId);
        return reserva == null ? null : _mapper.Map<ReservaDTO>(reserva);
    }

    public async Task<ReservaFormularioDTO> GetFormularioAsync(int sucursalId)
    {
        var sucursal = await _repository.FindSucursalAsync(sucursalId)
            ?? throw new KeyNotFoundException("La sucursal del encargado no existe.");

        return new ReservaFormularioDTO
        {
            SucursalNombre = sucursal.Nombre,
            Clientes = _mapper.Map<ICollection<UsuarioOpcionDTO>>(
                await _repository.ListClientesAsync()),
            Servicios = _mapper.Map<ICollection<ServicioDTO>>(
                await _repository.ListServiciosAsync())
        };
    }

    public async Task<(bool Exito, string Mensaje)> CreateAsync(
        ReservaCrearDTO dto, int sucursalId, int usuarioRegistroId)
    {
        if (dto.Fecha < DateOnly.FromDateTime(DateTime.Today))
            return (false, "La fecha de la reserva no puede ser anterior a la fecha actual.");

        if (dto.HoraInicio.Minute != 0)
            return (false, "La hora debe establecerse en horas completas.");

        var servicio = (await _repository.ListServiciosAsync())
            .FirstOrDefault(item => item.ServicioId == dto.ServicioId);
        if (servicio is null)
            return (false, "El servicio seleccionado no existe o está inactivo.");

        var inicio = dto.Fecha.ToDateTime(dto.HoraInicio);
        var fin = inicio.AddMinutes(servicio.DuracionMinutos);
        if (!await _repository.HorarioDisponibleAsync(sucursalId, inicio, fin))
            return (false, "La hora no está disponible, está fuera del horario o coincide con un bloqueo.");

        var reserva = new Reserva
        {
            CodigoReserva = $"RES-{Guid.NewGuid():N}"[..20].ToUpperInvariant(),
            ClienteId = dto.ClienteId,
            SucursalId = sucursalId,
            ServicioId = dto.ServicioId,
            EstadoReservaId = 1,
            FechaHoraInicio = inicio,
            FechaHoraFin = fin,
            TipoConsola = dto.TipoConsola.Trim(),
            ProblemaReportado = dto.ProblemaReportado.Trim(),
            EnciendeCorrectamente = dto.EnciendeCorrectamente,
            FueReparadaAnteriormente = dto.FueReparadaAnteriormente,
            HaceRuidoLectora = dto.HaceRuidoLectora,
            ObservacionesCliente = string.IsNullOrWhiteSpace(dto.ObservacionesCliente)
                ? null
                : dto.ObservacionesCliente.Trim(),
            NotificacionEnviada = false,
            UsuarioRegistroId = usuarioRegistroId,
            FechaRegistro = DateTime.Now
        };

        const decimal porcentajeImpuesto = 13m;
        var subtotal = servicio.Precio;
        var impuesto = Math.Round(subtotal * porcentajeImpuesto / 100m, 2);
        var proforma = new Factura
        {
            NumeroFactura = $"PRO-{Guid.NewGuid():N}"[..20].ToUpperInvariant(),
            ClienteId = dto.ClienteId,
            SucursalId = sucursalId,
            UsuarioRegistroId = usuarioRegistroId,
            FechaEmision = DateTime.Now,
            EsProforma = true,
            EstadoFactura = "Proforma",
            PorcentajeImpuesto = porcentajeImpuesto,
            Subtotal = subtotal,
            Impuesto = impuesto,
            Total = subtotal + impuesto,
            CorreoEnviado = false,
            Reserva = reserva
        };
        proforma.DetalleFactura.Add(new DetalleFactura
        {
            ServicioId = servicio.ServicioId,
            Descripcion = servicio.Nombre,
            Cantidad = 1,
            PrecioUnitario = servicio.Precio,
            PorcentajeImpuesto = porcentajeImpuesto
        });

        await _repository.AddAsync(reserva, proforma);
        return (true, $"Reserva {reserva.CodigoReserva} y factura proforma creadas correctamente.");
    }
}
