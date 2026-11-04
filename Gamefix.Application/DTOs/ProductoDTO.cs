using System.ComponentModel.DataAnnotations;

namespace Gamefix.Application.DTOs
{
    
        public record ProductoDTO
        {
            public int ProductoId { get; set; }

            [Range(1, int.MaxValue, ErrorMessage = "Seleccione una categoría.")]
            public int CategoriaProductoId { get; set; }

            public string CategoriaNombre { get; set; } = string.Empty;

            [Required, StringLength(120, MinimumLength = 2)]
            public string Nombre { get; set; } = string.Empty;

            [StringLength(500)]
            public string? Descripcion { get; set; }

            [StringLength(100)]
            public string? Marca { get; set; }

            [StringLength(200)]
            public string? Compatibilidad { get; set; }

            [Range(0, 999999999)]
            public decimal Precio { get; set; }

            [Range(0, int.MaxValue)]
            public int Existencia { get; set; }

            [Range(0, int.MaxValue)]
            public int StockMinimo { get; set; }

            public byte[]? Imagen { get; set; }

            public string? NombreImagen { get; set; }

            public string? TipoImagen { get; set; }

            public bool Activo { get; set; } = true;
        }
    }
