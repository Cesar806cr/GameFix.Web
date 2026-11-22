using System.ComponentModel.DataAnnotations;

namespace Gamefix.Application.DTOs;

public record SucursalDTO
{
    public int SucursalId { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio."), StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Display(Name = "Descripción")]
    [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres.")]
    public string? Descripcion { get; set; }

    [Display(Name = "Teléfono")]
    [Required(ErrorMessage = "El teléfono es obligatorio."), StringLength(25, ErrorMessage = "El teléfono no puede exceder 25 caracteres.")]
    public string Telefono { get; set; } = string.Empty;

    [Display(Name = "Dirección exacta")]
    [Required(ErrorMessage = "La dirección exacta es obligatoria."), StringLength(300, ErrorMessage = "La dirección exacta no puede exceder 300 caracteres.")]
    public string DireccionExacta { get; set; } = string.Empty;

    [Display(Name = "Correo electrónico")]
    [Required(ErrorMessage = "El correo electrónico es obligatorio."), EmailAddress(ErrorMessage = "Ingrese un correo electrónico válido."), StringLength(150, ErrorMessage = "El correo electrónico no puede exceder 150 caracteres.")]
    public string CorreoElectronico { get; set; } = string.Empty;





    public bool Activa { get; set; } = true;

    [Required(ErrorMessage = "El encargado es obligatorio, seleccione al menos un encargado.")]
    public List<int> EncargadoIds { get; set; } = [];
    public List<UsuarioOpcionDTO> EncargadosAsignados { get; set; } = [];
}

public record UsuarioOpcionDTO
{
    public int UsuarioId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string CorreoElectronico { get; set; } = string.Empty;
}
