using System;
using System.Collections.Generic;

namespace Gamefix.Infraestructure.Models;

public partial class Factura
{
    public int FacturaId { get; set; }

    public string NumeroFactura { get; set; } = null!;

    public int? ReservaId { get; set; }

    public int ClienteId { get; set; }

    public int SucursalId { get; set; }

    public int UsuarioRegistroId { get; set; }

    public DateTime FechaEmision { get; set; }

    public bool EsProforma { get; set; }

    public string EstadoFactura { get; set; } = null!;

    public decimal PorcentajeImpuesto { get; set; }

    public decimal Subtotal { get; set; }

    public decimal Impuesto { get; set; }

    public decimal Total { get; set; }

    public DateTime? FechaPago { get; set; }

    public bool CorreoEnviado { get; set; }

    public DateTime? FechaCorreoEnviado { get; set; }

    public string? RutaComprobantePdf { get; set; }

    public string? Observaciones { get; set; }

    public virtual Usuario Cliente { get; set; } = null!;

    public virtual ICollection<DetalleFactura> DetalleFactura { get; set; } = new List<DetalleFactura>();

    public virtual Reserva? Reserva { get; set; }

    public virtual Sucursal Sucursal { get; set; } = null!;

    public virtual Usuario UsuarioRegistro { get; set; } = null!;
}
