using Application.Defects.Commands;
using Application.Defects.Queries;
using Carter;
using Domain.Exceptions;
using MediatR;

namespace WebApi.Modules;

/// <summary>
/// Currently using FluentValidation, Carter, Problem details and MediatR behaviours together present some limitations. 
/// Because of this and to avoid hackish solutions, http responses are build manually instead of being infered by
/// exceptions. Using problem details is possible, but requires a hackish solution: https://github.com/khellang/Middleware/issues/157
/// </summary>
public class DefectModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/defect/", GetDefects);
        app.MapGet("api/defect/{machineId}/last", GetLastDefect);
        app.MapPost("api/defect", CreateDefect);
        app.MapPost("api/defect/status", SetDefectStatus);
    }

    private static async Task<IResult> GetDefects(IMediator mediator, CancellationToken cancellationToken)
    {
        try
        {
            return Results.Ok(await mediator.Send(new GetDefectCollectionQuery(), cancellationToken));
        }
        catch (Exception ex)
        {
            return Results.NotFound(ex.Message);
        }
    }

    private static async Task<IResult> GetLastDefect(int machineId, IMediator mediator, CancellationToken cancellationToken)
    {
        try
        {
            return Results.Ok(await mediator.Send(new GetLastDefectQuery(machineId), cancellationToken));
        }
        catch (Exception ex)
        {
            return Results.NotFound(ex.Message);
        }
    }

    private static async Task<IResult> CreateDefect(CreateDefectCommand command, IMediator mediator, CancellationToken cancellationToken)
    {
        try
        {
            var count = await mediator.Send(command, cancellationToken);
            if (count == 1)
            {
                return Results.Ok(new { success = "Successfully set the defect" });
            }
            return Results.StatusCode(StatusCodes.Status500InternalServerError);
        }
        catch (NotFoundException ex)
        {
            return Results.NotFound(ex.Message);
        }

    }

    private static async Task<IResult> SetDefectStatus(SetDefectStatusCommand command, IMediator mediator, CancellationToken cancellationToken)
    {
        try
        {
            return Results.Ok(await mediator.Send(command, cancellationToken));
        }
        catch(Exception ex)
        {
            return Results.NotFound(ex.Message);
        }
    }
}
