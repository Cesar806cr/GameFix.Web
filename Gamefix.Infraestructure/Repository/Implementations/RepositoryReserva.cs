using Gamefix.Infraestructure.Data;
using Gamefix.Infraestructure.Models;
using Gamefix.Infraestructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gamefix.Infraestructure.Repository.Implementations;

public class RepositoryReserva : IRepositoryReserva
{
    private readonly GameFixContext _context;

    public RepositoryReserva(GameFixContext context)
    {
        _context = context;
    }

    public async Task<ICollection<Reserva>> ListBySucursalAsync(int sucursalId, int? clienteId = null, DateTime? fecha = null)
    {
        var query = Query()
            .Where(reserva => reserva.SucursalId == sucursalId)
            .AsQueryable();

        if (clienteId.HasValue)
            query = query.Where(reserva => reserva.ClienteId == clienteId.Value);

        if (fecha.HasValue)
        {
            var siguiente = fecha.Value.Date.AddDays(1);
            query = query.Where(reserva =>
                reserva.FechaHoraInicio >= fecha.Value.Date &&
                reserva.FechaHoraInicio < siguiente);
        }

        return await query
            .OrderByDescending(reserva => reserva.FechaHoraInicio)
            .ToListAsync();
    }

    public async Task<Reserva?> FindByIdAsync(int id, int sucursalId)
    {
        return await Query()
            .FirstOrDefaultAsync(reserva =>
                reserva.ReservaId == id && reserva.SucursalId == sucursalId);
    }

    public async Task<ICollection<Usuario>> ListClientesAsync() =>
        await _context.Usuario
            .Where(usuario => usuario.RolId == 3 && usuario.Activo)
            .OrderBy(usuario => usuario.Nombre)
            .AsNoTracking()
            .ToListAsync();

    public async Task<ICollection<Servicio>> ListServiciosAsync() =>
        await _context.Servicio
            .Where(servicio => servicio.Activo)
            .OrderBy(servicio => servicio.Nombre)
            .AsNoTracking()
            .ToListAsync();

    public Task<Sucursal?> FindSucursalAsync(int sucursalId) =>
        _context.Sucursal
            .AsNoTracking()
            .FirstOrDefaultAsync(sucursal => sucursal.SucursalId == sucursalId && sucursal.Activa);

    public async Task<bool> HorarioDisponibleAsync(int sucursalId, DateTime inicio, DateTime fin)
    {
        var fecha = DateOnly.FromDateTime(inicio);
        var horaInicio = TimeOnly.FromDateTime(inicio);
        var horaFin = TimeOnly.FromDateTime(fin);
        var diaSemana = (byte)fecha.DayOfWeek;

        var horarioValido = await _context.HorarioSucursal.AnyAsync(horario =>
            horario.SucursalId == sucursalId &&
            horario.Activo &&
            horario.DiaSemana == diaSemana &&
            horario.HoraApertura <= horaInicio &&
            horario.HoraCierre >= horaFin);

        if (!horarioValido)
            return false;

        var bloqueo = await _context.BloqueoHorario.AnyAsync(item =>
            item.SucursalId == sucursalId &&
            item.Activo &&
            item.FechaInicio <= fecha &&
            item.FechaFin >= fecha &&
            (item.HoraInicio == null ||
             (item.HoraInicio < horaFin && item.HoraFin > horaInicio)));

        if (bloqueo)
            return false;

        return !await _context.Reserva.AnyAsync(reserva =>
            reserva.SucursalId == sucursalId &&
            reserva.EstadoReservaId != 5 &&
            reserva.FechaHoraInicio < fin &&
            reserva.FechaHoraFin > inicio);
    }

    public async Task AddAsync(Reserva reserva, Factura proforma)
    {
        proforma.Reserva = reserva;
        _context.Reserva.Add(reserva);
        _context.Factura.Add(proforma);
        await _context.SaveChangesAsync();
    }

    private IQueryable<Reserva> Query()
    {
        return _context.Reserva
            .Include(reserva => reserva.Cliente)
            .Include(reserva => reserva.Sucursal)
            .Include(reserva => reserva.Servicio)
            .Include(reserva => reserva.EstadoReserva)
            .AsNoTracking();
    }
}
