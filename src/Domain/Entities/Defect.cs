using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;
public class Defect
{
    // parameterless ctor needed for EF
    public Defect() { }
    public Defect(
        string personalNumber,
        string description,
        int machineId)
    {
        Id = -1;
        PersonalNumber = personalNumber;
        Description = description;
        Status = DefectStatus.Pending;
        MachineId = machineId;
        DefectTime = DateTime.UtcNow;
    }

    public int Id { get; private set; }

    [ForeignKey("Worker")]
    public string PersonalNumber { get; private set; }
    public string Description { get; private set; }
    public DefectStatus Status { get; private set; }
    public int MachineId { get; private set; }
    public DateTime DefectTime { get; private set; }

    public Worker Worker { get; private set; }
    public void SetStatus(DefectStatus status) => Status = status;
}

public enum DefectStatus
{
    Pending = 1,
    InProcess,
    Completed
}