using Gamefix.Application.DTOs;

namespace Gamefix.Application.Services.Interfaces;

public interface IServiceReserva
{
    Task<ICollection<ReservaDTO>> ListBySucursalAsync(int sucursalId, int? clienteId = null, DateTime? fecha = null);
    Task<ReservaDTO?> FindByIdAsync(int id, int sucursalId);
    Task<ReservaFormularioDTO> GetFormularioAsync(int sucursalId);
    Task<(bool Exito, string Mensaje)> CreateAsync(ReservaCrearDTO dto, int sucursalId, int usuarioRegistroId);
}
