using System;
using KpiBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace KpiBackend.Data;

public class KpiContext(DbContextOptions<KpiContext> options) : DbContext(options)
{
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<KpiAssignment> KpiAssignments => Set<KpiAssignment>();
    public DbSet<KpiMaster> KpiMasters => Set<KpiMaster>();
    public DbSet<KpiPeriod> KpiPeriods => Set<KpiPeriod>();
    public DbSet<KpiRealization> KpiRealizations => Set<KpiRealization>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //kalo department dihapus, user jangan ikut terhapus otomatis
        modelBuilder.Entity<User>()
        .HasOne(u => u.Department)
        .WithMany()
        .HasForeignKey(u=>u.DepartmentId)
        .OnDelete(DeleteBehavior.Restrict);

        //kalo manajer/user dihapus, managerId di department di set NUll
        modelBuilder.Entity<Department>()
        .HasOne(u =>u.Manager)
        .WithMany()
        .HasForeignKey(u=>u.ManagerId)
        .OnDelete(DeleteBehavior.SetNull);

        //kalo supervisor dihapus, supervisorId di set null
        modelBuilder.Entity<User>()
        .HasOne(u=>u.Supervisor)
        .WithMany()
        .HasForeignKey(u=>u.SupervisorId)
        .OnDelete(DeleteBehavior.Restrict);

        //buat email dan code kpi menjadi unique
        modelBuilder.Entity<User>().HasIndex(u =>u.Email).IsUnique();
        modelBuilder.Entity<KpiMaster>().HasIndex(u=>u.Code).IsUnique();

        //atur format
        modelBuilder.Entity<KpiAssignment>()
        .Property(a=>a.WeightPercentage).HasPrecision(5,2);
        modelBuilder.Entity<KpiAssignment>()
        .Property(a=>a.TargetValue).HasPrecision(15,2);
        modelBuilder.Entity<KpiRealization>()
        .Property(a=>a.ActualValue).HasPrecision(15,2);
        modelBuilder.Entity<KpiRealization>()
        .Property(a=>a.CalculatedScore).HasPrecision(8,2);

    }
}
