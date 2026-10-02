using System;
using System.Collections.Generic;

namespace Gamefix.Infraestructure.Models;

public partial class Reserva
{
    public int ReservaId { get; set; }

    public string CodigoReserva { get; set; } = null!;

    public int ClienteId { get; set; }

    public int SucursalId { get; set; }

    public int ServicioId { get; set; }

    public int EstadoReservaId { get; set; }

    public DateTime FechaHoraInicio { get; set; }

    public DateTime FechaHoraFin { get; set; }

    public string TipoConsola { get; set; } = null!;

    public string ProblemaReportado { get; set; } = null!;

    public bool EnciendeCorrectamente { get; set; }

    public bool FueReparadaAnteriormente { get; set; }

    public string? ObservacionesCliente { get; set; }

    public string? DiagnosticoEncargado { get; set; }

    public bool NotificacionEnviada { get; set; }

    public DateTime? FechaNotificacion { get; set; }

    public DateTime? FechaHoraAnterior { get; set; }

    public string? MotivoReprogramacion { get; set; }

    public int? UsuarioReprogramoId { get; set; }

    public int UsuarioRegistroId { get; set; }

    public DateTime FechaRegistro { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public virtual Usuario Cliente { get; set; } = null!;

    public virtual EstadoReserva EstadoReserva { get; set; } = null!;

    public virtual ICollection<Factura> Factura { get; set; } = new List<Factura>();

    public virtual ICollection<Notificacion> Notificacion { get; set; } = new List<Notificacion>();

    public virtual Servicio Servicio { get; set; } = null!;

    public virtual Sucursal Sucursal { get; set; } = null!;

    public virtual Usuario UsuarioRegistro { get; set; } = null!;

    public virtual Usuario? UsuarioReprogramo { get; set; }
}
