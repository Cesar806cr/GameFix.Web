using System;
using System.Collections.Generic;

namespace Gamefix.Infraestructure.Models;

public partial class Notificacion
{
    public int NotificacionId { get; set; }

    public int ReservaId { get; set; }

    public int ClienteId { get; set; }

    public string TipoNotificacion { get; set; } = null!;

    public string Medio { get; set; } = null!;

    public DateTime FechaProgramada { get; set; }

    public DateTime? FechaEnviada { get; set; }

    public bool Enviada { get; set; }

    public string Mensaje { get; set; } = null!;

    public string? DetalleError { get; set; }

    public virtual Usuario Cliente { get; set; } = null!;

    public virtual Reserva Reserva { get; set; } = null!;
}
