using System;
using System.Collections.Generic;

namespace Gamefix.Infraestructure.Models;

public partial class DetalleFactura
{
    public int DetalleFacturaId { get; set; }

    public int FacturaId { get; set; }

    public int? ServicioId { get; set; }

    public int? ProductoId { get; set; }

    public string Descripcion { get; set; } = null!;

    public decimal Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public decimal PorcentajeImpuesto { get; set; }

    public decimal? Subtotal { get; set; }

    public virtual Factura Factura { get; set; } = null!;

    public virtual Producto? Producto { get; set; }

    public virtual Servicio? Servicio { get; set; }
}
