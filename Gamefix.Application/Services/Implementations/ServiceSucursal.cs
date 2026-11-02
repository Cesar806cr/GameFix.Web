using AutoMapper;
using Gamefix.Application.DTOs;
using Gamefix.Application.Services.Interfaces;
using Gamefix.Infraestructure.Models;
using Gamefix.Infraestructure.Repository.Interfaces;

namespace Gamefix.Application.Services.Implementations;

public class ServiceSucursal : IServiceSucursal
{
    private readonly IRepositorySucursal _repository;
    private readonly IMapper _mapper;

    public ServiceSucursal(IRepositorySucursal repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ICollection<SucursalDTO>> ListAsync() =>
        _mapper.Map<ICollection<SucursalDTO>>(await _repository.ListAsync());

    public async Task<SucursalDTO?> FindByIdAsync(int id)
    {
        var entity = await _repository.FindByIdAsync(id);
        return entity is null ? null : _mapper.Map<SucursalDTO>(entity);
    }

    public async Task<ICollection<UsuarioOpcionDTO>> ListEncargadosDisponiblesAsync(int? sucursalId) =>
        _mapper.Map<ICollection<UsuarioOpcionDTO>>(
            await _repository.ListEncargadosDisponiblesAsync(sucursalId));

    public async Task AddAsync(SucursalDTO dto)
    {
        var entity = _mapper.Map<Sucursal>(dto);
        await _repository.AddAsync(entity, dto.EncargadoIds);
    }

    public async Task UpdateAsync(SucursalDTO dto)
    {
        var entity = _mapper.Map<Sucursal>(dto);
        await _repository.UpdateAsync(entity, dto.EncargadoIds);
    }
}
