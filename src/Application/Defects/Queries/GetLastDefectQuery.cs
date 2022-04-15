using Application.Common.Extensions;
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Defects.Queries;

public record GetLastDefectQuery(int MachineId) : IRequest<DefectDto>;

public class GetLastDefectQueryHandler : IRequestHandler<GetLastDefectQuery, DefectDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetLastDefectQueryHandler(
        IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<DefectDto> Handle(GetLastDefectQuery request, CancellationToken cancellationToken)
    {
        await _context.MachineExistsOrThrow(request.MachineId);

        return await _context.Defects
            .Where(c => c.MachineId == request.MachineId)
            .ProjectTo<DefectDto>(_mapper.ConfigurationProvider)
            .OrderByDescending(d => d.DefectTime)
            .FirstAsync(cancellationToken);
    }
}