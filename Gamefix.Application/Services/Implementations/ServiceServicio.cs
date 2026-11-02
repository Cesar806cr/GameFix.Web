using AutoMapper;
using Gamefix.Application.DTOs;
using Gamefix.Application.Services.Interfaces;
using Gamefix.Infraestructure.Repository.Interfaces;

namespace Gamefix.Application.Services.Implementations;

public class ServiceServicio : IServiceServicio
{
    private readonly IRepositoryServicio _repository;
    private readonly IMapper _mapper;

    public ServiceServicio(IRepositoryServicio repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ICollection<ServicioDTO>> ListAsync()
    {
        return _mapper.Map<ICollection<ServicioDTO>>(await _repository.ListAsync());
    }

    public async Task<ServicioDTO?> FindByIdAsync(int id)
    {
        var servicio = await _repository.FindByIdAsync(id);
        return servicio == null ? null : _mapper.Map<ServicioDTO>(servicio);
    }

    public async Task AddAsync(ServicioDTO dto)
    {
        var entity = _mapper.Map<Gamefix.Infraestructure.Models.Servicio>(dto);
        await _repository.AddAsync(entity);
    }

    public async Task UpdateAsync(ServicioDTO dto)
    {
        var entity = await _repository.FindByIdAsync(dto.ServicioId)
            ?? throw new KeyNotFoundException("El servicio no existe.");
        _mapper.Map(dto, entity);
        await _repository.UpdateAsync(entity);
    }
}
