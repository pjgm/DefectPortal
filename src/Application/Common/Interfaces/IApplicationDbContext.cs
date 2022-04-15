using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;
public interface IApplicationDbContext
{
    DbSet<Machine> Machines { get; }
    DbSet<Worker> Workers { get; }
    DbSet<Defect> Defects { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
