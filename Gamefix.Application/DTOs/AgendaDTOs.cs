using System.ComponentModel.DataAnnotations;

namespace Gamefix.Application.DTOs;

public record HorarioSucursalDTO
{
    public int HorarioSucursalId { get; set; }

    [Required(ErrorMessage = "Seleccione una sucursal.")]
    public int SucursalId { get; set; }

    [Required(ErrorMessage = "Seleccione un día de la semana.")]
    [Range(1, 6, ErrorMessage = "Seleccione un día de lunes a sábado")]
    public byte DiaSemana { get; set; }

    [Required(ErrorMessage = "La hora de apertura es obligatoria.")]
    public TimeOnly HoraApertura { get; set; }

    [Required(ErrorMessage = "La hora de cierre es obligatoria.")]
    public TimeOnly HoraCierre { get; set; }

    public bool Activo { get; set; } = true;
}

public record BloqueoHorarioDTO
{
    public int BloqueoHorarioId { get; set; }

    [Required(ErrorMessage = "Seleccione una sucursal.")]
    public int SucursalId { get; set; }

    [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
    public DateOnly FechaInicio { get; set; }

    [Required(ErrorMessage = "La fecha de fin es obligatoria.")]
    public DateOnly FechaFin { get; set; }

    public TimeOnly? HoraInicio { get; set; }
    public TimeOnly? HoraFin { get; set; }

    [Required(ErrorMessage = "El motivo es obligatorio."), StringLength(300, MinimumLength = 3, ErrorMessage = "El motivo debe tener entre 3 y 300 caracteres.")]
    public string Motivo { get; set; } = string.Empty;

    public bool Activo { get; set; } = true;
}

public record AgendaSucursalDTO
{
    public int SucursalId { get; set; }
    public string NombreSucursal { get; set; } = string.Empty;
    public List<HorarioSucursalDTO> Horarios { get; set; } = [];
    public List<BloqueoHorarioDTO> Bloqueos { get; set; } = [];
}
