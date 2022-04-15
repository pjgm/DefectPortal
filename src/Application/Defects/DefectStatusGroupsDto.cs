using Domain.Entities;

namespace Application.Defects;


public record DefectStatusGroupsDto(
    IEnumerable<DefectStatusGroupEntryDto> Pending,
    IEnumerable<DefectStatusGroupEntryDto> InProcess,
    IEnumerable<DefectStatusGroupEntryDto> Completed);
public record DefectStatusGroupEntryDto(
    int Id,
    string PersonalNumber,
    string Description,
    DefectStatus Status,
    int MachineId,
    DateTime DefectTime,
    string WorkerName);
