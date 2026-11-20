using System.ComponentModel.DataAnnotations;

namespace Gamefix.Application.DTOs;

public record FacturaDTO
{
    public int FacturaId { get; set; }
    public string NumeroFactura { get; set; } = string.Empty;
    public DateTime FechaEmision { get; set; }
    public string ClienteNombre { get; set; } = string.Empty;
    public string ClienteIdentificacion { get; set; } = string.Empty;
    public string ClienteCorreo { get; set; } = string.Empty;
    public string SucursalNombre { get; set; } = string.Empty;
    public string SucursalTelefono { get; set; } = string.Empty;
    public string SucursalDireccion { get; set; } = string.Empty;
    public string SucursalCorreo { get; set; } = string.Empty;
    public string EncargadoNombre { get; set; } = string.Empty;
    public bool EsProforma { get; set; }
    public string EstadoFactura { get; set; } = string.Empty;
    public decimal PorcentajeImpuesto { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Impuesto { get; set; }
    public decimal Total { get; set; }
    public List<FacturaDetalleDTO> Detalles { get; set; } = new();
}

public record FacturaFormularioDTO
{
    public FacturaCrearDTO Factura { get; set; } = new();
    public string SucursalNombre { get; set; } = string.Empty;
    public string EncargadoNombre { get; set; } = string.Empty;
    public ICollection<UsuarioOpcionDTO> Clientes { get; set; } = [];
    public ICollection<ServicioDTO> Servicios { get; set; } = [];
    public ICollection<ProductoDTO> Productos { get; set; } = [];
}

public record FacturaCrearDTO
{
    [Required(ErrorMessage = "Seleccione un cliente.")]
    [Range(1, int.MaxValue, ErrorMessage = "Seleccione un cliente.")]
    public int ClienteId { get; set; }
    public int? ReservaId { get; set; }

    [Required(ErrorMessage = "Seleccione un estado.")]
    [RegularExpression("Proforma|Pendiente|Pagada|Anulada",
        ErrorMessage = "Seleccione un estado válido.")]
    public string EstadoFactura { get; set; } = "Pendiente";

    public List<FacturaLineaDTO> Lineas { get; set; } = [];
}

public record FacturaLineaDTO
{
    public int? ServicioId { get; set; }
    public int? ProductoId { get; set; }

    [Range(1, 9999, ErrorMessage = "La cantidad debe ser un número entero entre 1 y 9999.")]
    public decimal Cantidad { get; set; } = 1;
}
