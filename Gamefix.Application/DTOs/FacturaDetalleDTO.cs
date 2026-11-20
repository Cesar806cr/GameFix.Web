namespace Gamefix.Application.DTOs;

public record FacturaDetalleDTO
{
    public int? ServicioId { get; set; }
    public int? ProductoId { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal { get; set; }
}
