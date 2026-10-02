using System;
using System.Collections.Generic;

namespace Gamefix.Infraestructure.Models;

public partial class EstadoReserva
{
    public int EstadoReservaId { get; set; }

    public string Nombre { get; set; } = null!;

    public string ColorHexadecimal { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<Reserva> Reserva { get; set; } = new List<Reserva>();
}
