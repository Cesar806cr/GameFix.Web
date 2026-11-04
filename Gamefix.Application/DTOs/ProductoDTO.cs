using System.ComponentModel.DataAnnotations;
using System.Timers;

namespace Gamefix.Application.DTOs
{
    
        public record ProductoDTO
        {
        public int ProductoId { get; set; }

        [Required(ErrorMessage = "Selecione una categoria")]
        public int CategoriaProductoId { get; set; }

        public string CategoriaNombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es obligatorio."),
         StringLength(120, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 4 y 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "La descripción no puede superar los 500 caracteres.")]
        public string? Descripcion { get; set; }

        [StringLength(100, ErrorMessage = "El nombre de la marca no puede superar los 100 caracteres.")]
        public string? Marca { get; set; }

        [StringLength(200, ErrorMessage = "La compatibilidad no puede superar los 200 caracteres.")]
        [Required(ErrorMessage = "Coloque una consola compatible")]
        public string? Compatibilidad { get; set; }

        [Range(0, 999999999, ErrorMessage = "El precio debe estar entre 0 y 999,999,999.")]
        [Required(ErrorMessage = "El precio es requerido")]
        public decimal Precio { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "La existencia no puede ser negativa.")]
        [Required(ErrorMessage = "La existencia es requerida")]
        public int Existencia { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El valor no puede ser negativo.")]
        [Required(ErrorMessage = "La existencia mínima es requereida")]
        public int StockMinimo { get; set; }

            public byte[]? Imagen { get; set; }

            public string? NombreImagen { get; set; }

            public string? TipoImagen { get; set; }

            public bool Activo { get; set; } = true;
        }
    }
