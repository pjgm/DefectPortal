using Application.Common.Interfaces;
using Bogus;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;
public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public static readonly int[] MachineIds = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
    public static readonly string[] WorkerIds = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Defect> Defects { get; set; }
    public DbSet<Machine> Machines { get; set; }
    public DbSet<Worker> Workers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        SeedWorkers(modelBuilder);
        SeedMachines(modelBuilder);
        SeedDefects(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }

    private static void SeedWorkers(ModelBuilder modelBuilder)
    {
        var workerId = 1;
        var workerFaker = new Faker<Worker>()
            .RuleFor(o => o.PersonalNumber, f => workerId++.ToString())
            .RuleFor(o => o.Name, f => f.Name.FullName());


        modelBuilder.Entity<Worker>(builder =>
        {
            builder.HasKey(o => o.PersonalNumber);
            builder.HasData(workerFaker.Generate(10));
        });
    }

    private static void SeedMachines(ModelBuilder modelBuilder)
    {
        int machineId = 1;
        var machineFaker = new Faker<Machine>()
            .RuleFor(o => o.MachineId, f => machineId++)
            .RuleFor(o => o.MachineType, f => Guid.NewGuid().ToString())
            .RuleFor(o => o.Shift, f => f.Random.Number(0, 10))
            .RuleFor(o => o.Status, f => f.Random.Number(0, 3))
            .RuleFor(o => o.SelectedOrders, f => f.Make(5, () => f.Random.String()))
            .RuleFor(o => o.Workplace, f => f.Random.Number(0, 15))
            .RuleFor(o => o.ParallelOrders, f => f.Random.Bool());

        var fakeMachineList = machineFaker.Generate(1000);

        modelBuilder.Entity<Machine>(builder => {
            builder
                .Ignore(c => c.SelectedOrders)
                .HasData(fakeMachineList);
        });
    }

    private static void SeedDefects(ModelBuilder modelBuilder)
    {
        var defectId = 1;

        var defectFaker = new Faker<Defect>()
            .RuleFor(o => o.Id, f => defectId++)
            .RuleFor(o => o.PersonalNumber, f => f.PickRandom(WorkerIds))
            .RuleFor(o => o.Description, f => f.Lorem.Sentence())
            .RuleFor(o => o.Status, f => f.PickRandom<DefectStatus>())
            .RuleFor(o => o.MachineId, f => f.PickRandom(MachineIds))
            .RuleFor(o => o.DefectTime, f => f.Date.Past());

        var fakeDetectList = defectFaker.Generate(1000);

        modelBuilder.Entity<Defect>(builder =>
        {
            //builder.HasOne<Worker>();
            builder.HasData(fakeDetectList);
        });
    }


}
