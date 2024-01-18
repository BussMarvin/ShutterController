using Database.Contract.DataModels;
using Microsoft.EntityFrameworkCore;

namespace Database;

public partial class ControlDatabaseContext : DbContext
{
    public ControlDatabaseContext()
    {
    }

    public ControlDatabaseContext(DbContextOptions<ControlDatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ControlTime_DataModel> ControlTimes { get; set; }

    public virtual DbSet<ControlTrace_DataModel> ControlTraces { get; set; }

    public virtual DbSet<Engine_DataModel> Engines { get; set; }

    public virtual DbSet<Equipment_DataModel> Equipment { get; set; }

    public virtual DbSet<Function_DataModel> Functions { get; set; }

    public virtual DbSet<FunctiongroupHb_DataModel> FunctiongroupHbs { get; set; }

    public virtual DbSet<FunctiongroupHe_DataModel> FunctiongroupHes { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ControlTime_DataModel>(entity =>
        {
            entity.HasKey(e => e.Guid);

            entity.ToTable("ControlTime");

            entity.HasOne(d => d.FunctionNavigation).WithMany(p => p.ControlTimes).HasForeignKey(d => d.Function);

            entity.HasOne(d => d.FunctiongroupNavigation).WithMany(p => p.ControlTimes).HasForeignKey(d => d.Functiongroup);
        });

        modelBuilder.Entity<ControlTrace_DataModel>(entity =>
        {
            entity.HasKey(e => e.Guid);

            entity.ToTable("ControlTrace");

            entity.HasOne(d => d.FunctionNavigation).WithMany(p => p.ControlTraces).HasForeignKey(d => d.Function);

            entity.HasOne(d => d.FunctiongroupHbsNavigation).WithMany(p => p.ControlTraces).HasForeignKey(d => d.FunctiongroupHbs);

            entity.HasOne(d => d.FunctiongroupHeNavigation).WithMany(p => p.ControlTraces).HasForeignKey(d => d.FunctiongroupHe);
        });

        modelBuilder.Entity<Engine_DataModel>(entity =>
        {
            entity.ToTable("Engine");

            entity.HasIndex(e => e.Id, "IX_Engine_Id").IsUnique();

            entity.HasOne(d => d.NameNavigation).WithMany(p => p.EngineNameNavigations).HasForeignKey(d => d.Name);

            entity.HasOne(d => d.RelayLeftNavigation).WithMany(p => p.EngineRelayLeftNavigations).HasForeignKey(d => d.RelayLeft);

            entity.HasOne(d => d.RelayRightNavigation).WithMany(p => p.EngineRelayRightNavigations).HasForeignKey(d => d.RelayRight);
        });

        modelBuilder.Entity<Equipment_DataModel>(entity => { entity.HasIndex(e => e.Id, "IX_Equipment_Id").IsUnique(); });

        modelBuilder.Entity<Function_DataModel>(entity =>
        {
            entity.ToTable("Function");

            entity.HasIndex(e => e.Id, "IX_Function_Id").IsUnique();
        });

        modelBuilder.Entity<FunctiongroupHb_DataModel>(entity =>
        {
            entity.HasIndex(e => e.Id, "IX_FunctiongroupHbs_Id").IsUnique();

            entity.HasOne(d => d.ButtonNavigation).WithMany(p => p.FunctiongroupHbs).HasForeignKey(d => d.Button);
        });

        modelBuilder.Entity<FunctiongroupHe_DataModel>(entity =>
        {
            entity.ToTable("FunctiongroupHe");

            entity.HasIndex(e => e.Id, "IX_FunctiongroupHe_Id").IsUnique();

            entity.HasOne(d => d.EngineNavigation).WithMany(p => p.FunctiongroupHes).HasForeignKey(d => d.Engine);

            entity.HasOne(d => d.PositionSensorBottomNavigation).WithMany(p => p.FunctiongroupHePositionSensorBottomNavigations)
                .HasForeignKey(d => d.PositionSensorBottom);

            entity.HasOne(d => d.PositionSensorTopNavigation).WithMany(p => p.FunctiongroupHePositionSensorTopNavigations)
                .HasForeignKey(d => d.PositionSensorTop);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}