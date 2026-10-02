using System;
using System.Collections.Generic;

namespace Gamefix.Infraestructure.Models;

public partial class Sucursal
{
    public int SucursalId { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public string Telefono { get; set; } = null!;

    public string DireccionExacta { get; set; } = null!;

    public string CorreoElectronico { get; set; } = null!;

    public bool Activa { get; set; }

    public DateTime FechaRegistro { get; set; }

    public virtual ICollection<BloqueoHorario> BloqueoHorario { get; set; } = new List<BloqueoHorario>();

    public virtual ICollection<Factura> Factura { get; set; } = new List<Factura>();

    public virtual ICollection<HorarioSucursal> HorarioSucursal { get; set; } = new List<HorarioSucursal>();

    public virtual ICollection<Reserva> Reserva { get; set; } = new List<Reserva>();

    public virtual ICollection<Usuario> Usuario { get; set; } = new List<Usuario>();
}
