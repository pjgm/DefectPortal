namespace Domain.Entities;
public class Worker
{
    // parameterless ctor needed for EF
    public Worker() { }
    public Worker(string personalNumber, string name)
    {
        PersonalNumber = personalNumber;
        Name = name;
    }

    public string PersonalNumber { get; private set; }
    public string Name { get; private set; }

    public ICollection<Defect> Books { get; private set; }
}
