using Gamefix.Infraestructure.Data;
using Gamefix.Infraestructure.Models;
using Gamefix.Infraestructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gamefix.Infraestructure.Repository.Implementations;

public class RepositoryAgenda : IRepositoryAgenda
{
    private readonly GameFixContext _context;

    public RepositoryAgenda(GameFixContext context) => _context = context;

    public async Task<ICollection<Sucursal>> ListSucursalesAsync() =>
        await _context.Sucursal
            .Where(sucursal => sucursal.Activa)
            .OrderBy(sucursal => sucursal.Nombre)
            .AsNoTracking()
            .ToListAsync();

    public Task<HorarioSucursal?> FindHorarioAsync(int id) =>
        _context.HorarioSucursal
            .Include(horario => horario.Sucursal)
            .FirstOrDefaultAsync(horario => horario.HorarioSucursalId == id);

    public Task<BloqueoHorario?> FindBloqueoAsync(int id) =>
        _context.BloqueoHorario
            .Include(bloqueo => bloqueo.Sucursal)
            .FirstOrDefaultAsync(bloqueo => bloqueo.BloqueoHorarioId == id);

    public async Task<ICollection<HorarioSucursal>> ListHorariosAsync(int sucursalId) =>
        await _context.HorarioSucursal
            .Where(horario => horario.SucursalId == sucursalId && horario.Activo)
            .OrderBy(horario => horario.DiaSemana)
            .AsNoTracking()
            .ToListAsync();

    public async Task<ICollection<BloqueoHorario>> ListBloqueosAsync(int sucursalId) =>
        await _context.BloqueoHorario
            .Where(bloqueo => bloqueo.SucursalId == sucursalId && bloqueo.Activo)
            .OrderBy(bloqueo => bloqueo.FechaInicio)
            .AsNoTracking()
            .ToListAsync();

    public async Task AddHorarioAsync(HorarioSucursal entity)
    {
        _context.HorarioSucursal.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task AddBloqueoAsync(BloqueoHorario entity)
    {
        entity.UsuarioRegistroId = await _context.Usuario
            .Where(usuario => usuario.Activo)
            .Select(usuario => usuario.UsuarioId)
            .FirstAsync();
        _context.BloqueoHorario.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> HorarioOverlapsAsync(HorarioSucursal item) =>
        await _context.HorarioSucursal.AnyAsync(other =>
            other.SucursalId == item.SucursalId &&
            other.Activo &&
            other.DiaSemana == item.DiaSemana &&
            other.HoraApertura < item.HoraCierre &&
            other.HoraCierre > item.HoraApertura);

    public async Task<bool> BloqueoOverlapsAsync(BloqueoHorario item)
    {
        var existing = await _context.BloqueoHorario
            .Where(other => other.SucursalId == item.SucursalId && other.Activo)
            .ToListAsync();

        return existing.Any(other =>
            other.FechaInicio <= item.FechaFin &&
            other.FechaFin >= item.FechaInicio &&
            RangesOverlap(other.HoraInicio, other.HoraFin, item.HoraInicio, item.HoraFin));
    }

    public async Task<bool> HorarioBloqueoOverlapsAsync(HorarioSucursal item)
    {
        var blocks = await _context.BloqueoHorario
            .Where(block => block.SucursalId == item.SucursalId && block.Activo)
            .ToListAsync();

        return blocks.Any(block =>
            DatesMatchDay(block.FechaInicio, block.FechaFin, item.DiaSemana) &&
            RangesOverlap(block.HoraInicio, block.HoraFin, item.HoraApertura, item.HoraCierre));
    }

    public async Task<bool> BloqueoHorarioOverlapsAsync(BloqueoHorario item)
    {
        var schedules = await _context.HorarioSucursal
            .Where(schedule => schedule.SucursalId == item.SucursalId && schedule.Activo)
            .ToListAsync();

        return schedules.Any(schedule =>
            DatesMatchDay(item.FechaInicio, item.FechaFin, schedule.DiaSemana) &&
            RangesOverlap(item.HoraInicio, item.HoraFin, schedule.HoraApertura, schedule.HoraCierre));
    }

    private static bool DatesMatchDay(DateOnly start, DateOnly end, byte day)
    {
        for (var date = start; date <= end; date = date.AddDays(1))
        {
            if ((byte)date.DayOfWeek == day)
                return true;
        }

        return false;
    }

    private static bool RangesOverlap(TimeOnly? firstStart, TimeOnly? firstEnd,
        TimeOnly? secondStart, TimeOnly? secondEnd)
    {
        if (firstStart is null || secondStart is null)
            return true;

        return firstStart.Value < secondEnd!.Value &&
               firstEnd!.Value > secondStart.Value;
    }
}
