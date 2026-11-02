using Gamefix.Application.DTOs;

namespace Gamefix.Application.Services.Interfaces;

public interface IServiceAgenda
{
    Task<ICollection<SucursalDTO>> ListSucursalesAsync();
    Task<AgendaSucursalDTO?> GetAgendaAsync(int sucursalId);
    Task<HorarioSucursalDTO?> FindHorarioAsync(int id);
    Task<BloqueoHorarioDTO?> FindBloqueoAsync(int id);
    Task<(bool Exito, string Mensaje)> AddHorarioAsync(HorarioSucursalDTO dto);
    Task<(bool Exito, string Mensaje)> AddBloqueoAsync(BloqueoHorarioDTO dto);
}
