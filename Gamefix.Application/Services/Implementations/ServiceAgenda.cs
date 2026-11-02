using AutoMapper;
using Gamefix.Application.DTOs;
using Gamefix.Application.Services.Interfaces;
using Gamefix.Infraestructure.Models;
using Gamefix.Infraestructure.Repository.Interfaces;

namespace Gamefix.Application.Services.Implementations;

public class ServiceAgenda : IServiceAgenda
{
    private readonly IRepositoryAgenda _repository;
    private readonly IMapper _mapper;

    public ServiceAgenda(IRepositoryAgenda repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ICollection<SucursalDTO>> ListSucursalesAsync() =>
        _mapper.Map<ICollection<SucursalDTO>>(await _repository.ListSucursalesAsync());

    public async Task<AgendaSucursalDTO?> GetAgendaAsync(int sucursalId)
    {
        var sucursal = (await _repository.ListSucursalesAsync())
            .FirstOrDefault(item => item.SucursalId == sucursalId);
        if (sucursal is null)
            return null;

        return new AgendaSucursalDTO
        {
            SucursalId = sucursal.SucursalId,
            NombreSucursal = sucursal.Nombre,
            Horarios = _mapper.Map<List<HorarioSucursalDTO>>(
                await _repository.ListHorariosAsync(sucursalId)),
            Bloqueos = _mapper.Map<List<BloqueoHorarioDTO>>(
                await _repository.ListBloqueosAsync(sucursalId))
        };
    }

    public async Task<HorarioSucursalDTO?> FindHorarioAsync(int id) =>
        _mapper.Map<HorarioSucursalDTO>(await _repository.FindHorarioAsync(id));

    public async Task<BloqueoHorarioDTO?> FindBloqueoAsync(int id) =>
        _mapper.Map<BloqueoHorarioDTO>(await _repository.FindBloqueoAsync(id));

    public async Task<(bool Exito, string Mensaje)> AddHorarioAsync(HorarioSucursalDTO dto)
    {
        if (dto.HoraCierre <= dto.HoraApertura)
            return (false, "La hora de cierre debe ser posterior a la hora de apertura.");
        if (dto.HoraApertura.Minute != 0 || dto.HoraCierre.Minute != 0)
            return (false, "Los horarios deben utilizar horas completas.");
        if (await _repository.HorarioOverlapsAsync(_mapper.Map<HorarioSucursal>(dto)))
            return (false, "Existe una sobreposición de horarios.");
        if (await _repository.HorarioBloqueoOverlapsAsync(_mapper.Map<HorarioSucursal>(dto)))
            return (false, "Existe una sobreposición de horarios con bloqueos.");

        await _repository.AddHorarioAsync(_mapper.Map<HorarioSucursal>(dto));
        return (true, "Horario creado correctamente.");
    }

    public async Task<(bool Exito, string Mensaje)> AddBloqueoAsync(BloqueoHorarioDTO dto)
    {
        if (dto.FechaInicio < DateOnly.FromDateTime(DateTime.Today))
            return (false, "La fecha no puede ser anterior a la fecha actual.");
        if (dto.FechaFin < dto.FechaInicio)
            return (false, "La fecha final debe ser igual o posterior a la fecha inicial.");
        if ((dto.HoraInicio is null) != (dto.HoraFin is null))
            return (false, "Debe indicar ambas horas o dejar el bloqueo para todo el día.");
        if (dto.HoraInicio is not null &&
            (dto.HoraInicio.Value.Minute != 0 || dto.HoraFin!.Value.Minute != 0 ||
             dto.HoraFin <= dto.HoraInicio))
            return (false, "El bloqueo debe utilizar un rango de horas completas válido.");

        var entity = _mapper.Map<BloqueoHorario>(dto);
        if (await _repository.BloqueoOverlapsAsync(entity))
            return (false, "Existe una sobreposición de bloqueos.");
        if (await _repository.BloqueoHorarioOverlapsAsync(entity))
            return (false, "Existe una sobreposición de horarios con bloqueos.");

        await _repository.AddBloqueoAsync(entity);
        return (true, "Bloqueo creado correctamente.");
    }
}
