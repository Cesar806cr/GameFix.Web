using AutoMapper;
using Gamefix.Application.DTOs;
using Gamefix.Application.Services.Interfaces;
using Gamefix.Infraestructure.Models;
using Gamefix.Infraestructure.Repository.Interfaces;

namespace Gamefix.Application.Services.Implementations;

public class ServiceFactura : IServiceFactura
{
    private readonly IRepositoryFactura _repository;
    private readonly IMapper _mapper;

    public ServiceFactura(IRepositoryFactura repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ICollection<FacturaDTO>> ListAsync(int sucursalId, int? clienteId = null, DateTime? fecha = null)
    {
        return _mapper.Map<ICollection<FacturaDTO>>(
            await _repository.ListAsync(sucursalId, clienteId, fecha));
    }

    public async Task<FacturaDTO?> FindByIdAsync(int id, int sucursalId)
    {
        var factura = await _repository.FindByIdAsync(id, sucursalId);
        return factura == null ? null : _mapper.Map<FacturaDTO>(factura);
    }

    public async Task<FacturaFormularioDTO> GetFormularioAsync(
        int sucursalId, int usuarioRegistroId, int? proformaId = null)
    {
        var sucursal = await _repository.FindSucursalAsync(sucursalId)
            ?? throw new KeyNotFoundException("La sucursal del encargado no existe.");
        var formulario = new FacturaFormularioDTO
        {
            SucursalNombre = sucursal.Nombre,
            EncargadoNombre = (await _repository.FindUsuarioAsync(usuarioRegistroId))?.Nombre
                ?? throw new KeyNotFoundException("El encargado no existe o está inactivo."),
            Clientes = _mapper.Map<ICollection<UsuarioOpcionDTO>>(
                await _repository.ListClientesAsync()),
            Servicios = _mapper.Map<ICollection<ServicioDTO>>(
                await _repository.ListServiciosAsync()),
            Productos = _mapper.Map<ICollection<ProductoDTO>>(
                await _repository.ListProductosAsync())
        };

        if (proformaId.HasValue)
        {
            var proforma = await _repository.FindByIdAsync(proformaId.Value, sucursalId)
                ?? throw new KeyNotFoundException("La factura proforma no existe.");
            if (!proforma.EsProforma)
                throw new InvalidOperationException("La factura seleccionada ya está facturada.");

            formulario.Factura = new FacturaCrearDTO
            {
                ClienteId = proforma.ClienteId,
                ReservaId = proforma.ReservaId,
                EstadoFactura = "Proforma",
                Lineas = proforma.DetalleFactura
                    .Select(detalle => new FacturaLineaDTO
                    {
                        ServicioId = detalle.ServicioId,
                        ProductoId = detalle.ProductoId,
                        Cantidad = detalle.Cantidad
                    })
                    .ToList()
            };
        }

        return formulario;
    }

    public async Task<(bool Exito, string Mensaje)> CreateAsync(
        FacturaCrearDTO dto, int sucursalId, int usuarioRegistroId)
    {
        if (dto.Lineas is null || dto.Lineas.Count == 0)
            return (false, "Seleccione al menos un servicio y coloque una cantidad válida.");

        if (!dto.Lineas.Any(linea =>
            linea.ServicioId.HasValue &&
            linea.Cantidad >= 1 &&
            linea.Cantidad <= 9999 &&
            linea.Cantidad == decimal.Truncate(linea.Cantidad)))
            return (false, "Seleccione al menos un servicio y coloque una cantidad válida.");

        if (dto.Lineas.Any(linea =>
            linea.Cantidad < 1 ||
            linea.Cantidad > 9999 ||
            linea.Cantidad != decimal.Truncate(linea.Cantidad)))
            return (false, "La cantidad debe ser un número entero entre 1 y 9999.");

        var estadosPermitidos = new[]
        {
            "Proforma", "Pendiente", "Pagada", "Anulada"
        };
        if (!estadosPermitidos.Contains(dto.EstadoFactura))
            return (false, "El estado seleccionado no es válido.");

        var productosDisponibles = (await _repository.ListProductosAsync())
            .ToDictionary(item => item.ProductoId);
        var productoSinExistencia = dto.Lineas.FirstOrDefault(linea =>
            linea.ProductoId.HasValue &&
            productosDisponibles.TryGetValue(linea.ProductoId.Value, out var producto) &&
            linea.Cantidad > producto.Existencia);
        if (productoSinExistencia is not null)
            return (false, "La cantidad solicitada supera la existencia disponible del producto.");

        var lineas = await BuildDetailsAsync(dto.Lineas);
        if (lineas.Count == 0)
            return (false, "Agregue al menos un producto o servicio.");

        var subtotal = lineas.Sum(linea => linea.Cantidad * linea.PrecioUnitario);
        const decimal porcentajeImpuesto = 13m;
        var impuesto = Math.Round(subtotal * porcentajeImpuesto / 100m, 2);
        var factura = new Factura
        {
            NumeroFactura = await _repository.GenerateNumeroAsync(
                dto.EstadoFactura == "Proforma", sucursalId),
            ReservaId = dto.ReservaId,
            ClienteId = dto.ClienteId,
            SucursalId = sucursalId,
            UsuarioRegistroId = usuarioRegistroId,
            FechaEmision = DateTime.Now,
            EsProforma = dto.EstadoFactura == "Proforma",
            EstadoFactura = dto.EstadoFactura,
            PorcentajeImpuesto = porcentajeImpuesto,
            Subtotal = subtotal,
            Impuesto = impuesto,
            Total = subtotal + impuesto,
            FechaPago = dto.EstadoFactura == "Pagada" ? DateTime.Now : null,
            CorreoEnviado = false
        };
        factura.DetalleFactura = lineas;

        await _repository.AddAsync(factura);
        return (true, $"La factura {factura.NumeroFactura} fue registrada correctamente.");
    }

    public async Task<(bool Exito, string Mensaje)> ConvertirProformaAsync(
        int id, int sucursalId, int usuarioRegistroId)
    {
        var factura = await _repository.FindByIdAsync(id, sucursalId);
        if (factura is null)
            return (false, "La factura proforma no existe.");
        if (!factura.EsProforma)
            return (false, "La factura seleccionada ya está facturada.");

        factura.EsProforma = false;
        factura.EstadoFactura = "Pendiente";
        factura.FechaEmision = DateTime.Now;
        factura.FechaPago = null;
        factura.UsuarioRegistroId = usuarioRegistroId;
        factura.NumeroFactura = await _repository.GenerateNumeroAsync(false, sucursalId);
        await _repository.UpdateAsync(factura);
        return (true, $"La factura {factura.NumeroFactura} fue registrada correctamente.");
    }

    public async Task<(bool Exito, string Mensaje)> UpdateEstadoAsync(
        int id, string estado, int sucursalId)
    {
        var factura = await _repository.FindByIdAsync(id, sucursalId);
        if (factura is null)
            return (false, "La factura no existe.");

        var estadosPermitidos = new[]
        {
            "Proforma", "Pendiente", "Pagada", "Anulada"
        };
        if (!estadosPermitidos.Contains(estado))
            return (false, "El estado seleccionado no es válido.");

        if (!factura.EsProforma && estado == "Proforma")
            return (false, "Una factura registrada no puede volver a Proforma.");

        if (factura.EsProforma && estado != "Proforma")
        {
            factura.NumeroFactura = await _repository.GenerateNumeroAsync(false, sucursalId);
            factura.EsProforma = false;
        }

        factura.EstadoFactura = estado;
        factura.FechaPago = estado == "Pagada" ? DateTime.Now : null;
        await _repository.UpdateAsync(factura);
        return (true, $"El estado se cambió a {estado}.");
    }

    private async Task<List<DetalleFactura>> BuildDetailsAsync(
        IEnumerable<FacturaLineaDTO> lineas)
    {
        var servicios = (await _repository.ListServiciosAsync())
            .ToDictionary(item => item.ServicioId);
        var productos = (await _repository.ListProductosAsync())
            .ToDictionary(item => item.ProductoId);
        var detalles = new List<DetalleFactura>();

        foreach (var linea in lineas.Where(item => item.Cantidad > 0))
        {
            if (linea.ServicioId.HasValue && servicios.TryGetValue(linea.ServicioId.Value, out var servicio))
            {
                detalles.Add(new DetalleFactura
                {
                    ServicioId = servicio.ServicioId,
                    Descripcion = servicio.Nombre,
                    Cantidad = linea.Cantidad,
                    PrecioUnitario = servicio.Precio,
                    PorcentajeImpuesto = 13m
                });
            }
            else if (linea.ProductoId.HasValue && productos.TryGetValue(linea.ProductoId.Value, out var producto))
            {
                detalles.Add(new DetalleFactura
                {
                    ProductoId = producto.ProductoId,
                    Descripcion = producto.Nombre,
                    Cantidad = linea.Cantidad,
                    PrecioUnitario = producto.Precio,
                    PorcentajeImpuesto = 13m
                });
            }
        }

        return detalles;
    }
}
