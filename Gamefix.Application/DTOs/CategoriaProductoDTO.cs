namespace Gamefix.Application.DTOs;

public record CategoriaProductoDTO
{
    public int CategoriaProductoId { get; set; }
    public string Nombre { get; set; } = string.Empty;
}
