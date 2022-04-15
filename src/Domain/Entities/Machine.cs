namespace Domain.Entities;

public class Machine
{
    // parameterless ctor needed for EF
    public Machine() { }
    public Machine(
        int machineId,
        string machineType,
        int shift,
        int status,
        List<string> selectedOrders,
        int workplace,
        bool parallelOrders)
    {
        MachineId = machineId;
        MachineType = machineType;
        Shift = shift;
        Status = status;
        SelectedOrders = selectedOrders;
        Workplace = workplace;
        ParallelOrders = parallelOrders;
    }

    public int MachineId { get; private set; }
    public string MachineType { get; private set; }
    public int Shift { get; private set; }
    public int Status { get; private set; }
    public List<string> SelectedOrders { get; private set; }
    public int Workplace { get; private set; }
    public bool ParallelOrders { get; private set; }

    public void SetStatus(int status) => Status = status;
}
