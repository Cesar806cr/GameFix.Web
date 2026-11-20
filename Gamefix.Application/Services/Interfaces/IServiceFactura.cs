using Gamefix.Application.DTOs;

namespace Gamefix.Application.Services.Interfaces;

public interface IServiceFactura
{
    Task<ICollection<FacturaDTO>> ListAsync(int sucursalId, int? clienteId = null, DateTime? fecha = null);
    Task<FacturaDTO?> FindByIdAsync(int id, int sucursalId);
    Task<FacturaFormularioDTO> GetFormularioAsync(int sucursalId, int usuarioRegistroId, int? proformaId = null);
    Task<(bool Exito, string Mensaje)> CreateAsync(FacturaCrearDTO dto, int sucursalId, int usuarioRegistroId);
    Task<(bool Exito, string Mensaje)> ConvertirProformaAsync(int id, int sucursalId, int usuarioRegistroId);
    Task<(bool Exito, string Mensaje)> UpdateEstadoAsync(int id, string estado, int sucursalId);
}
