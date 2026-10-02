using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamefix.Application.DTOs
{
    
        public record ProductoDTO
        {
            public int ProductoId { get; set; }

            public int CategoriaProductoId { get; set; }

            public string Nombre { get; set; } = string.Empty; 

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
        }
    }


