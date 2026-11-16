namespace ReservasDeportivas.Domain.Entities;

public class Actividad
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public int DuracionMinutos { get; set; }
    public decimal Precio { get; set; }
    public string ImagenUrl { get; set; } = string.Empty;
    public bool Activa { get; set; } = true;
}
