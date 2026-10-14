using AutoMapper;
using Gamefix.Application.DTOs;
using Gamefix.Application.Services.Interfaces;
using Gamefix.Infraestructure.Repository.Interfaces;

namespace Gamefix.Application.Services.Implementations;

public class ServiceReserva : IServiceReserva
{
    private readonly IRepositoryReserva _repository;
    private readonly IMapper _mapper;

    public ServiceReserva(IRepositoryReserva repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ICollection<ReservaDTO>> ListBySucursalAsync(int sucursalId)
    {
        return _mapper.Map<ICollection<ReservaDTO>>(
            await _repository.ListBySucursalAsync(sucursalId));
    }

    public async Task<ReservaDTO?> FindByIdAsync(int id, int sucursalId)
    {
        var reserva = await _repository.FindByIdAsync(id, sucursalId);
        return reserva == null ? null : _mapper.Map<ReservaDTO>(reserva);
    }
}
