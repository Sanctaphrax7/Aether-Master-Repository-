using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Aether.Shared.Models;

public partial class ForgeContext : DbContext
{
    public ForgeContext()
    {
    }

    public ForgeContext(DbContextOptions<ForgeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BudgetDataTemp> BudgetDataTemps { get; set; }

    public virtual DbSet<BudgetDatum> BudgetData { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server= MIS-MARCELT;Database=Forge;Integrated Security=True;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BudgetDataTemp>(entity =>
        {
            entity.HasKey(e => new { e.CompanyNo, e.Division, e.GlDeptNo, e.GlAccountNo, e.SubAccountNo, e.FiscalYear, e.CalMonth, e.RevisionNo }).HasName("PK__BudgetDa__BAE359154A1424C5");

            entity.ToTable("BudgetDataTemp");

            entity.Property(e => e.Division)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.GlDeptNo)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.GlAccountNo)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.SubAccountNo)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.LastUpdated).HasColumnType("date");
            entity.Property(e => e.PerAmt).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<BudgetDatum>(entity =>
        {
            entity.HasKey(e => new { e.Division, e.GlAccountNo, e.GlDeptNo, e.SubAccountNo, e.CompanyNo, e.FiscalYear, e.CalMonth, e.RevisionNo }).HasName("PK__BudgetDa__E2853A488A572FD6");

            entity.Property(e => e.Division)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.GlAccountNo)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.GlDeptNo)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.SubAccountNo)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.FileName).IsUnicode(false);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.LastUpdated).HasColumnType("datetime");
            entity.Property(e => e.PerAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UpdatedBy).IsUnicode(false);

            entity.HasOne(d => d.User).WithMany(p => p.BudgetData)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BudgetDat__UserI__17F790F9");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Roles__3214EC07E06A44C6");

            entity.Property(e => e.Roles)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC27C673ED97");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UserName).HasMaxLength(50);
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserRole__3214EC0760F8A825");

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("RoleId");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UserId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
