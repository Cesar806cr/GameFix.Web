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
}
