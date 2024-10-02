using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DigiPetsBackEnd.Models;

public partial class DigiPetsDbContext : DbContext
{
    public DigiPetsDbContext()
    {
    }

    public DigiPetsDbContext(DbContextOptions<DigiPetsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Pet> Pets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Data Source=.\\sqlexpress;Initial Catalog=DigiPetsDB; Integrated Security=SSPI;Encrypt=false;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Pets__3214EC27B4D57754");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Health).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
