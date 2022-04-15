using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Defects.Commands;

public record SetDefectStatusCommand(
    int MachineId,
    DateTime DefectTime,
    DefectStatus Status) : IRequest<Unit>;

public class SetDefectCommandHandler : IRequestHandler<SetDefectStatusCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    public SetDefectCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task<Unit> Handle(SetDefectStatusCommand request, CancellationToken cancellationToken)
    {
        var defect = await _context.Defects.FirstAsync(d => 
            d.MachineId == request.MachineId &&
            d.DefectTime == request.DefectTime,
            cancellationToken);

        defect.SetStatus(request.Status);

        var machine = await _context.Machines.FirstAsync(m =>
            m.MachineId == request.MachineId,
            cancellationToken);

        if (request.Status == DefectStatus.Completed)
        {
            machine.SetStatus(1);
        }

        _context.Defects.Update(defect);
        _context.Machines.Update(machine);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
