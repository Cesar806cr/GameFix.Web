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
}
