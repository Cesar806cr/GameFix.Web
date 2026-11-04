using System.ComponentModel.DataAnnotations;

namespace Gamefix.Application.DTOs;

public record ServicioDTO
{
    public int ServicioId { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio."),
     StringLength(120, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 120 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "La descripción no puede superar los 500 caracteres.")]
    public string? Descripcion { get; set; }

    [Range(0, 999999999, ErrorMessage = "El precio debe estar entre 0 y 999,999,999.")]
    [Required(ErrorMessage = "El precio es requerido")]
    public decimal Precio { get; set; }

    [Range(1, 1440, ErrorMessage = "La duración del servicio tiene que estar entre 1 y 1440 minutos.")]
    [Required(ErrorMessage = "La duración del servicio es requerida")]
    public int DuracionMinutos { get; set; }

    [Required(ErrorMessage = "Escriba el nombre de una consola compatible"),
     StringLength(150, ErrorMessage = "La consola compatible no puede superar los 150 caracteres.")]
    public string ConsolaCompatible { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nivel de dificultad es obligatorio.")]
    public string NivelDificultad { get; set; } = string.Empty;

    public bool RequiereDiagnostico { get; set; }

    [Range(0, 3650, ErrorMessage = "El valor debe estar entre 0 y 3650.")]
    [Required(ErrorMessage = "La garantía en días es requerida")]
    public int GarantiaDias { get; set; }
    public byte[]? Imagen { get; set; }
    public string? NombreImagen { get; set; }
    public string? TipoImagen { get; set; }
    public bool Activo { get; set; } = true;
}
