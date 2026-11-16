using System.ComponentModel.DataAnnotations;

namespace ReservasDeportivas.Application.DTOs;

public class ReservacionCrearDto
{
    [Range(1, int.MaxValue,
        ErrorMessage = "Debe seleccionar una actividad.")]
    public int ActividadId { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, MinimumLength = 3,
        ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres.")]
    public string NombreCliente { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "El correo no es válido.")]
    public string Correo { get; set; } = string.Empty;

    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    [Phone(ErrorMessage = "El teléfono no es válido.")]
    [StringLength(20)]
    public string Telefono { get; set; } = string.Empty;

    [Required(ErrorMessage = "La fecha es obligatoria.")]
    public DateTime? FechaActividad { get; set; }

    [Required(ErrorMessage = "Seleccione una hora.")]
    public string HoraInicio { get; set; } = string.Empty;
}
