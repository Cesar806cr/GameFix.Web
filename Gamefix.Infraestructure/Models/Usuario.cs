using System;
using System.Collections.Generic;

namespace Gamefix.Infraestructure.Models;

public partial class Usuario
{
    public int UsuarioId { get; set; }

    public int RolId { get; set; }

    public int? SucursalId { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Identificacion { get; set; }

    public string Telefono { get; set; } = null!;

    public string CorreoElectronico { get; set; } = null!;

    public string DireccionExacta { get; set; } = null!;

    public DateOnly FechaNacimiento { get; set; }

    public string ContrasenaHash { get; set; } = null!;

    public bool Activo { get; set; }

    public DateTime FechaRegistro { get; set; }

    public DateTime? FechaUltimoAcceso { get; set; }

    public virtual ICollection<BloqueoHorario> BloqueoHorario { get; set; } = new List<BloqueoHorario>();

    public virtual ICollection<Factura> FacturaCliente { get; set; } = new List<Factura>();

    public virtual ICollection<Factura> FacturaUsuarioRegistro { get; set; } = new List<Factura>();

    public virtual ICollection<Notificacion> Notificacion { get; set; } = new List<Notificacion>();

    public virtual ICollection<Reserva> ReservaCliente { get; set; } = new List<Reserva>();

    public virtual ICollection<Reserva> ReservaUsuarioRegistro { get; set; } = new List<Reserva>();

    public virtual ICollection<Reserva> ReservaUsuarioReprogramo { get; set; } = new List<Reserva>();

    public virtual Rol Rol { get; set; } = null!;

    public virtual Sucursal? Sucursal { get; set; }
}
