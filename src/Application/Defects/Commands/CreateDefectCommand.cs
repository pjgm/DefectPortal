using Application.Common.Extensions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Defects.Commands;

public record CreateDefectCommand(
    string PersonalNumber, 
    string Description, 
    int MachineId) : IRequest<int>;

public class CreateDefectCommandHandler : IRequestHandler<CreateDefectCommand, int>
{
    private readonly IApplicationDbContext _context;
    public CreateDefectCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task<int> Handle(CreateDefectCommand request, CancellationToken cancellationToken)
    {
        await _context.WorkerExistsOrThrow(request.PersonalNumber);
        await _context.MachineExistsOrThrow(request.MachineId);

        await _context.Defects.AddAsync(
            new Defect(
                request.PersonalNumber,
                request.Description,
                request.MachineId),
            cancellationToken);

        return await _context.SaveChangesAsync(cancellationToken);
    }
}

