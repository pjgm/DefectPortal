using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Defects.Queries;

public record GetDefectCollectionQuery() : IRequest<DefectStatusGroupsDto>;

public class GetDefectQueryHandler : IRequestHandler<GetDefectCollectionQuery, DefectStatusGroupsDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetDefectQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<DefectStatusGroupsDto> Handle(GetDefectCollectionQuery request, CancellationToken cancellationToken)
    {
        var result = (await _context.Defects
            .Include(d => d.Worker)
            .ToListAsync(cancellationToken))
            .GroupBy(d => d.Status);

        return _mapper.Map<DefectStatusGroupsDto>(result);
    }

}