namespace Gamefix.Application.DTOs;

public record ServicioDTO
{
    public int ServicioId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public decimal Precio { get; set; }
    public int DuracionMinutos { get; set; }
    public string ConsolaCompatible { get; set; } = string.Empty;
    public string NivelDificultad { get; set; } = string.Empty;
    public bool RequiereDiagnostico { get; set; }
    public int GarantiaDias { get; set; }
    public byte[]? Imagen { get; set; }
    public string? NombreImagen { get; set; }
    public string? TipoImagen { get; set; }
}
