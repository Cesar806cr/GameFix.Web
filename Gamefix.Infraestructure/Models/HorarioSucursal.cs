using System;
using System.Collections.Generic;

namespace Gamefix.Infraestructure.Models;

public partial class HorarioSucursal
{
    public int HorarioSucursalId { get; set; }

    public int SucursalId { get; set; }

    public byte DiaSemana { get; set; }

    public TimeOnly HoraApertura { get; set; }

    public TimeOnly HoraCierre { get; set; }

    public bool Activo { get; set; }

    public virtual Sucursal Sucursal { get; set; } = null!;
}
