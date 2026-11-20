using System.ComponentModel.DataAnnotations;

namespace Gamefix.Application.DTOs;

public record ReservaDTO
{
    public int ReservaId { get; set; }
    public string CodigoReserva { get; set; } = string.Empty;
    public DateTime FechaRegistro { get; set; }
    public DateTime FechaHoraInicio { get; set; }
    public DateTime FechaHoraFin { get; set; }
    public string SucursalNombre { get; set; } = string.Empty;
    public string ServicioNombre { get; set; } = string.Empty;
    public string ClienteNombre { get; set; } = string.Empty;
    public string TipoConsola { get; set; } = string.Empty;
    public string ProblemaReportado { get; set; } = string.Empty;
    public bool EnciendeCorrectamente { get; set; }
    public bool FueReparadaAnteriormente { get; set; }
    public bool HaceRuidoLectora { get; set; }
    public string? ObservacionesCliente { get; set; }
    public string EstadoNombre { get; set; } = string.Empty;
    public string EstadoColor { get; set; } = "#6c757d";
    public string ClienteCorreo { get; set; } = string.Empty;
    public int ClienteId { get; set; }
    public int ServicioId { get; set; }
    public decimal ServicioPrecio { get; set; }
    public int DuracionMinutos { get; set; }
}

public record ReservaFormularioDTO
{
    public ReservaCrearDTO Reserva { get; set; } = new();
    public string SucursalNombre { get; set; } = string.Empty;
    public ICollection<UsuarioOpcionDTO> Clientes { get; set; } = [];
    public ICollection<ServicioDTO> Servicios { get; set; } = [];
}

public record ReservaCrearDTO
{
    [Required(ErrorMessage = "Seleccione un cliente.")]
    [Range(1, int.MaxValue, ErrorMessage = "Seleccione un cliente.")]
    public int ClienteId { get; set; }

    [Required(ErrorMessage = "Seleccione un servicio.")]
    [Range(1, int.MaxValue, ErrorMessage = "Seleccione un servicio.")]
    public int ServicioId { get; set; }

    [Required(ErrorMessage = "Seleccione una fecha.")]
    public DateOnly Fecha { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    [Required(ErrorMessage = "Seleccione una hora.")]
    public TimeOnly HoraInicio { get; set; }

    [Required(ErrorMessage = "Indique la consola.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "La consola debe tener entre 2 y 100 caracteres.")]
    public string TipoConsola { get; set; } = string.Empty;

    [Required(ErrorMessage = "Describa el problema.")]
    [StringLength(1000, MinimumLength = 5, ErrorMessage = "El problema debe tener entre 5 y 1000 caracteres.")]
    public string ProblemaReportado { get; set; } = string.Empty;

    public bool EnciendeCorrectamente { get; set; }
    public bool FueReparadaAnteriormente { get; set; }
    public bool HaceRuidoLectora { get; set; }

    [StringLength(1000, ErrorMessage = "Las observaciones no pueden superar 1000 caracteres.")]
    public string? ObservacionesCliente { get; set; }
}
