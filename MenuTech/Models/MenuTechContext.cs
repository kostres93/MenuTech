using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace MenuTech.Models
{
    public partial class MenuTechContext : DbContext
    {
        public MenuTechContext()
        {
        }

        public MenuTechContext(DbContextOptions<MenuTechContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CurrencyRate> CurrencyRates { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<CustomerAccount> CustomerAccounts { get; set; }
        public virtual DbSet<StoreAccount> StoreAccounts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseSqlServer(Configuration.GetConnectionString("ConnectionString"));
                // optionsBuilder.UseSqlServer("Server=DESKTOP-UF0483H\\SQLEXPRESS01;Database=MenuTech;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<CurrencyRate>(entity =>
            {
                entity.HasKey(e => e.IdCurrencyRate)
                    .HasName("PK__Currency__91E4E504033C734B");

                entity.ToTable("CurrencyRate");

                entity.Property(e => e.Currency)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.CurrencyRateValue)
                    .HasColumnType("decimal(19, 2)")
                    .HasColumnName("CurrencyRateValue");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");

                entity.Property(e => e.Password)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CustomerAccount>(entity =>
            {
                entity.HasKey(e => e.IdCustomerAccount)
                    .HasName("PK__Customer__9D092F260693B608");

                entity.ToTable("CustomerAccount");

                entity.Property(e => e.Balance).HasColumnType("decimal(19, 2)");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CustomerAccounts)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK__CustomerA__Custo__267ABA7A");
            });

            modelBuilder.Entity<StoreAccount>(entity =>
            {
                entity.HasKey(e => e.IdStoreAccount)
                    .HasName("PK__StoreAcc__7CFD9D3854130109");

                entity.ToTable("StoreAccount");

                entity.Property(e => e.Balance).HasColumnType("decimal(19, 2)");

                entity.Property(e => e.TransactionId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.TransactionMinus).HasColumnType("decimal(19, 2)");

                entity.Property(e => e.TransactionPlus).HasColumnType("decimal(19, 2)");

                entity.Property(e => e.Refund).HasColumnType("int");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.StoreAccounts)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK__StoreAcco__Custo__4AB81AF0");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
