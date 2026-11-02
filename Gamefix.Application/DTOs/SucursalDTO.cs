using System.ComponentModel.DataAnnotations;

namespace Gamefix.Application.DTOs;

public record SucursalDTO
{
    public int SucursalId { get; set; }

    [Required, StringLength(100, MinimumLength = 3)]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Descripcion { get; set; }

    [Required, StringLength(25)]
    public string Telefono { get; set; } = string.Empty;

    [Required, StringLength(300)]
    public string DireccionExacta { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(150)]
    public string CorreoElectronico { get; set; } = string.Empty;

    public bool Activa { get; set; } = true;
    public List<int> EncargadoIds { get; set; } = [];
    public List<UsuarioOpcionDTO> EncargadosAsignados { get; set; } = [];
}

public record UsuarioOpcionDTO
{
    public int UsuarioId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string CorreoElectronico { get; set; } = string.Empty;
}
