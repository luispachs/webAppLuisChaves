using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebAppluisChaves.Models
{
    public partial class billing_luis_chavesContext : DbContext
    {
        public billing_luis_chavesContext()
        {
        }

        public billing_luis_chavesContext(DbContextOptions<billing_luis_chavesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Bill> Bills { get; set; } = null!;
        public virtual DbSet<City> Cities { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<State> States { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("name=sqlServerConnections");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bill>(entity =>
            {
                entity.ToTable("bills");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Amount)
                    .HasColumnName("amount")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.IdUser).HasColumnName("id_user");

                entity.Property(e => e.ListObject)
                    .HasColumnType("text")
                    .HasColumnName("list_object");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Bills)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__bills__id_user__30F848ED");
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("cities");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CityName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("city_name");

                entity.Property(e => e.StateId).HasColumnName("state_id");

                entity.HasOne(d => d.State)
                    .WithMany(p => p.Cities)
                    .HasForeignKey(d => d.StateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__cities__state_id__31EC6D26");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("products");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Amount)
                    .HasColumnName("amount")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Discount)
                    .HasColumnName("discount")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Price)
                    .HasColumnName("price")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ProductName)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("product_name");
            });

            modelBuilder.Entity<State>(entity =>
            {
                entity.ToTable("states");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code).HasColumnName("code");

                entity.Property(e => e.StateName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("state_name");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasIndex(e => e.Nit, "UQ__users__DF97D0E4EEC4B9E9")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BusinessName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("business_name");

                entity.Property(e => e.City).HasColumnName("city");

                entity.Property(e => e.LocalAddress)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("local_address");

                entity.Property(e => e.Nit).HasColumnName("nit");

                entity.Property(e => e.Phone)
                    .HasMaxLength(18)
                    .IsUnicode(false)
                    .HasColumnName("phone");

                entity.HasOne(d => d.CityNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.City)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__users__city__32E0915F");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
