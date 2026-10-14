using AutoMapper;
using Gamefix.Application.DTOs;
using Gamefix.Application.Services.Interfaces;
using Gamefix.Infraestructure.Repository.Interfaces;

namespace Gamefix.Application.Services.Implementations;

public class ServiceFactura : IServiceFactura
{
    private readonly IRepositoryFactura _repository;
    private readonly IMapper _mapper;

    public ServiceFactura(IRepositoryFactura repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ICollection<FacturaDTO>> ListAsync()
    {
        return _mapper.Map<ICollection<FacturaDTO>>(await _repository.ListAsync());
    }

    public async Task<FacturaDTO?> FindByIdAsync(int id)
    {
        var factura = await _repository.FindByIdAsync(id);
        return factura == null ? null : _mapper.Map<FacturaDTO>(factura);
    }
}
