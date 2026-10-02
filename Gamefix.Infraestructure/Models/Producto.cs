using System;
using System.Collections.Generic;

namespace Gamefix.Infraestructure.Models;

public partial class Producto
{
    public int ProductoId { get; set; }

    public int CategoriaProductoId { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public string? Marca { get; set; }

    public string? Compatibilidad { get; set; }

    public decimal Precio { get; set; }

    public int Existencia { get; set; }

    public int StockMinimo { get; set; }

    public byte[]? Imagen { get; set; }

    public string? NombreImagen { get; set; }

    public string? TipoImagen { get; set; }

    public bool Activo { get; set; }

    public DateTime FechaRegistro { get; set; }

    public virtual CategoriaProducto CategoriaProducto { get; set; } = null!;

    public virtual ICollection<DetalleFactura> DetalleFactura { get; set; } = new List<DetalleFactura>();
}
