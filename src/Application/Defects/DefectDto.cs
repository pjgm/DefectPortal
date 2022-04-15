using Domain.Entities;

namespace Application.Defects;

public record DefectDto(int Id, string PersonalNumber, string Description, DefectStatus Status, int MachineId, DateTime DefectTime);
