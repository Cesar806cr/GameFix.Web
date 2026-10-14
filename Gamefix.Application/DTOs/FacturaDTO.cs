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
    public string EstadoFactura { get; set; } = string.Empty;
    public decimal PorcentajeImpuesto { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Impuesto { get; set; }
    public decimal Total { get; set; }
    public List<FacturaDetalleDTO> Detalles { get; set; } = new();
}
