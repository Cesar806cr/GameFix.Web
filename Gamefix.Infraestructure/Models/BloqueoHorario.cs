using System;
using System.Collections.Generic;

namespace Gamefix.Infraestructure.Models;

public partial class BloqueoHorario
{
    public int BloqueoHorarioId { get; set; }

    public int SucursalId { get; set; }

    public DateOnly FechaInicio { get; set; }

    public DateOnly FechaFin { get; set; }

    public TimeOnly? HoraInicio { get; set; }

    public TimeOnly? HoraFin { get; set; }

    public string Motivo { get; set; } = null!;

    public bool Activo { get; set; }

    public int UsuarioRegistroId { get; set; }

    public DateTime FechaRegistro { get; set; }

    public virtual Sucursal Sucursal { get; set; } = null!;

    public virtual Usuario UsuarioRegistro { get; set; } = null!;
}
