using System;
using System.Collections.Generic;

namespace Gamefix.Infraestructure.Models;

public partial class CategoriaProducto
{
    public int CategoriaProductoId { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public bool Activo { get; set; }

    public virtual ICollection<Producto> Producto { get; set; } = new List<Producto>();
}
