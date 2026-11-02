using System.ComponentModel.DataAnnotations;

namespace Gamefix.Application.DTOs;

public record HorarioSucursalDTO
{
    public int HorarioSucursalId { get; set; }
    public int SucursalId { get; set; }

    [Range(1, 6, ErrorMessage = "Seleccione un día de lunes a sábado.")]
    public byte DiaSemana { get; set; }

    [Required]
    public TimeOnly HoraApertura { get; set; }

    [Required]
    public TimeOnly HoraCierre { get; set; }

    public bool Activo { get; set; } = true;
}

public record BloqueoHorarioDTO
{
    public int BloqueoHorarioId { get; set; }
    public int SucursalId { get; set; }

    [Required]
    public DateOnly FechaInicio { get; set; }

    [Required]
    public DateOnly FechaFin { get; set; }

    public TimeOnly? HoraInicio { get; set; }
    public TimeOnly? HoraFin { get; set; }

    [Required, StringLength(300, MinimumLength = 3)]
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
