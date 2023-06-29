using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace recommender.DataProvide
{
    public partial class RecommenderDBContext : DbContext
    {
        public RecommenderDBContext()
        {
        }

        public RecommenderDBContext(DbContextOptions<RecommenderDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Car> Cars { get; set; }
        public virtual DbSet<FuelType> FuelTypes { get; set; }
        public virtual DbSet<OwnerType> OwnerTypes { get; set; }
        public virtual DbSet<Transmission> Transmissions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySQL("Server=10.51.10.137;Port=3333;Database=RecommenderDB;Uid=user;Pwd=recommender");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Car>(entity =>
            {
                entity.ToTable("car");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.FuelTypeId).HasColumnName("fuelTypeId");

                entity.Property(e => e.KmDriven).HasColumnName("kmDriven");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("name");

                entity.Property(e => e.OwnerTypeId).HasColumnName("ownerTypeId");

                entity.Property(e => e.SellingPrice).HasColumnName("sellingPrice");

                entity.Property(e => e.TransmissionId).HasColumnName("transmissionId");

                entity.Property(e => e.Year).HasColumnName("year");
            });

            modelBuilder.Entity<FuelType>(entity =>
            {
                entity.ToTable("fuelType");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<OwnerType>(entity =>
            {
                entity.ToTable("ownerType");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Transmission>(entity =>
            {
                entity.ToTable("transmission");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
