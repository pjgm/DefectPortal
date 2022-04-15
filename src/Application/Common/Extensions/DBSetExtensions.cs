using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Extensions;

//TODO: this can be replaced by async FluentValidation methods. Would make this code cleaner
public static class DBSetExtensions
{
    public static async Task MachineExistsOrThrow(this IApplicationDbContext context, int machineId)
    {
        var machineExists = await context
            .Machines
            .AnyAsync(m => m.MachineId == machineId);

        if (!machineExists)
        {
            throw new NotFoundException($"{nameof(Machine)} {machineId} does not exist");
        }
    }

    public static async Task WorkerExistsOrThrow(this IApplicationDbContext context, string personalNumber)
    {
        var workerExists = await context
            .Workers
            .AnyAsync(w => w.PersonalNumber == personalNumber);

        if (!workerExists)
        {
            throw new NotFoundException($"{nameof(Worker)} {personalNumber} does not exist");
        }
    }
}
