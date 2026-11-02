using System.ComponentModel.DataAnnotations;

namespace Gamefix.Application.DTOs;

public record ServicioDTO
{
    public int ServicioId { get; set; }
    [Required, StringLength(120, MinimumLength = 3)]
    public string Nombre { get; set; } = string.Empty;
    [StringLength(500)]
    public string? Descripcion { get; set; }
    [Range(0, 999999999)]
    public decimal Precio { get; set; }
    [Range(60, 1440, ErrorMessage = "La duracion del servicio tiene que estar entre 60 y 1440 minutos.")]
    public int DuracionMinutos { get; set; }
    [Required, StringLength(150)]
    public string ConsolaCompatible { get; set; } = string.Empty;
    [Required]
    public string NivelDificultad { get; set; } = string.Empty;
    public bool RequiereDiagnostico { get; set; }
    [Range(0, 3650)]
    public int GarantiaDias { get; set; }
    public byte[]? Imagen { get; set; }
    public string? NombreImagen { get; set; }
    public string? TipoImagen { get; set; }
}
