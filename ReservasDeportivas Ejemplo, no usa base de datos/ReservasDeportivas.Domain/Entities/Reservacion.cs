namespace ReservasDeportivas.Domain.Entities;

public class Reservacion
{
    public int Id { get; set; }
    public int ActividadId { get; set; }

    public string NombreCliente { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;

    public DateTime FechaSolicitud { get; set; }
    public DateTime Inicio { get; set; }
    public DateTime Fin { get; set; }

    public string Estado { get; set; } = "Confirmada";
}
