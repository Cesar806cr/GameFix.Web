using Gamefix.Infraestructure.Models;

namespace Gamefix.Infraestructure.Repository.Interfaces;

public interface IRepositoryAgenda
{
    Task<ICollection<Sucursal>> ListSucursalesAsync();
    Task<HorarioSucursal?> FindHorarioAsync(int id);
    Task<BloqueoHorario?> FindBloqueoAsync(int id);
    Task<ICollection<HorarioSucursal>> ListHorariosAsync(int sucursalId);
    Task<ICollection<BloqueoHorario>> ListBloqueosAsync(int sucursalId);
    Task AddHorarioAsync(HorarioSucursal entity);
    Task AddBloqueoAsync(BloqueoHorario entity);
    Task<bool> HorarioOverlapsAsync(HorarioSucursal item);
    Task<bool> BloqueoOverlapsAsync(BloqueoHorario item);
    Task<bool> HorarioBloqueoOverlapsAsync(HorarioSucursal item);
    Task<bool> BloqueoHorarioOverlapsAsync(BloqueoHorario item);
}
