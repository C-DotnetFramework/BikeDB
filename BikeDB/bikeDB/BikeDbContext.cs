using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BikeDB.bikeDB;

public partial class BikeDbContext : DbContext
{
    public BikeDbContext()
    {
    }

    public BikeDbContext(DbContextOptions<BikeDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Bike> Bikes { get; set; }

    public virtual DbSet<RentalHistory> RentalHistories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("server=localhost;database=bikeDB;uid=root;pwd=@ssm501799");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bike>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("bike");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Used).HasColumnName("used");
        });

        modelBuilder.Entity<RentalHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("rental_history");

            entity.HasIndex(e => e.BikeId, "bike_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BikeId).HasColumnName("bike_id");
            entity.Property(e => e.RentalTime)
                .HasColumnType("datetime")
                .HasColumnName("rental_time");
            entity.Property(e => e.ReturnTime)
                .HasColumnType("datetime")
                .HasColumnName("return_time");

            entity.HasOne(d => d.Bike).WithMany(p => p.RentalHistories)
                .HasForeignKey(d => d.BikeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("rental_history_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
