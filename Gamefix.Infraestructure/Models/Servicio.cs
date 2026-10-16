using System;
using System.Collections.Generic;

namespace Gamefix.Infraestructure.Models;

public partial class Servicio
{
    public int ServicioId { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public decimal Precio { get; set; }

    public int DuracionMinutos { get; set; }

    public string ConsolaCompatible { get; set; } = null!;

    public string NivelDificultad { get; set; } = null!;

    public bool RequiereDiagnostico { get; set; }

    public int GarantiaDias { get; set; }

    public byte[]? Imagen { get; set; }

    public string? NombreImagen { get; set; }

    public string? TipoImagen { get; set; }

    public bool Activo { get; set; }

    public DateTime FechaRegistro { get; set; }

    public virtual ICollection<DetalleFactura> DetalleFactura { get; set; } = new List<DetalleFactura>();

    public virtual ICollection<Reserva> Reserva { get; set; } = new List<Reserva>();
}
