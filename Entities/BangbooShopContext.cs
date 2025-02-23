using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using YourNamespace.Models;

namespace Bangboo_E_Commerce.Entities;

public partial class BangbooShopContext : DbContext
{
    public BangbooShopContext()
    {
    }

    public BangbooShopContext(DbContextOptions<BangbooShopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BangbooDb> BangbooDbs { get; set; }

    public virtual DbSet<BangbooImageDb> BangbooImageDbs { get; set; }

    public DbSet<BoorderDetail> BoorderDetails { get; set; }

    public virtual DbSet<Boorder> Boorders { get; set; }

    public virtual DbSet<CartBoo> CartBoos { get; set; }

    public virtual DbSet<PhaethonUser> PhaethonUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=LAPTOP-CO730HHP;Initial Catalog=BangbooShop;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BangbooDb>(entity =>
        {
            entity.HasKey(e => e.Idbangboo).HasName("PK_Product_DB");

            entity.ToTable("Bangboo_DB");

            entity.Property(e => e.Idbangboo)
                .HasMaxLength(50)
                .HasColumnName("IDBangboo");
            entity.Property(e => e.Attribute).HasMaxLength(50);
            entity.Property(e => e.Faction).HasMaxLength(50);
            entity.Property(e => e.NameBangboo).HasMaxLength(50);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Rank).HasMaxLength(50);
        });

        modelBuilder.Entity<BangbooImageDb>(entity =>
        {
            entity.HasKey(e => e.IDImage).HasName("PK_ProductImage_DB");

            entity.ToTable("BangbooImage_DB");

            entity.Property(e => e.IDImage)
                .HasMaxLength(50)
                .HasColumnName("IDImage");
            entity.Property(e => e.IDImage_Bangboo)
                .HasMaxLength(50)
                .HasColumnName("IDImage_Bangboo");
            entity.Property(e => e.Url).HasColumnName("URL");

            entity.HasOne(d => d.IdimageBangbooNavigation).WithMany(p => p.BangbooImageDbs)
                .HasForeignKey(d => d.IDImage_Bangboo)
                .HasConstraintName("FK_BangbooImage_DB_Bangboo_DB1");
        });

        modelBuilder.Entity<BoorderDetail>(entity =>
        {
            entity.HasKey(e => e.IDOrderDetail);

            entity.ToTable("BooderDetail");

            entity.Property(e => e.IDOrderDetail)
                .HasColumnName("IDOrderDetail")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.IDOrder_DB)
                .HasColumnName("IDOrder_DB")
                .IsRequired();

            entity.Property(e => e.IDBangboo)
                .HasColumnName("IDBangboo")
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.Quantity)
                .HasColumnName("Quantity")
                .IsRequired();

            entity.Property(e => e.Unit_Price)
                .HasColumnName("Unit_Price")
                .HasColumnType("decimal(18, 0)")
                .IsRequired();

            entity.Property(e => e.Total_Price)
                .HasColumnName("Total_Price")
                .HasColumnType("decimal(18, 0)")
                .IsRequired();

            entity.HasOne(e => e.Boorder)
                .WithMany(b => b.BoorderDetails)
                .HasForeignKey(e => e.IDOrder_DB)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_BoorderDetail_Boorder");

            entity.HasOne(e => e.Bangboo)
                .WithMany(b => b.BoorderDetails)
                .HasForeignKey(e => e.IDBangboo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BoorderDetail_Bangboo");
        });

        modelBuilder.Entity<Boorder>(entity =>
        {
            entity.HasKey(e => e.Idorder);

            entity.ToTable("Boorder");

            entity.Property(e => e.Idorder).HasColumnName("IDOrder");
            entity.Property(e => e.IdPhaethon)
                .HasMaxLength(50)
                .HasColumnName("ID_Phaethon");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(50)
                .HasColumnName("Payment_Method");
            entity.Property(e => e.SellerInfo)
                .HasMaxLength(100)
                .HasColumnName("Seller_Info");
            entity.Property(e => e.TransactionId)
                .HasMaxLength(100)
                .HasColumnName("Transaction_ID");
            entity.Property(e => e.TotalAmount)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("Total_Amount");
            entity.HasOne(d => d.IdPhatheonNavigation).WithMany(p => p.Boorders)
                .HasForeignKey(d => d.IdPhaethon)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Boorder_Phaethon_User");
        });

        modelBuilder.Entity<CartBoo>(entity =>
        {
            entity.HasKey(e => e.IdCartBoo);

            entity.ToTable("CartBoo");

            entity.Property(e => e.IdCartBoo).HasColumnName("ID_CartBoo");
            entity.Property(e => e.IdOrder).HasColumnName("ID_Order");
            entity.Property(e => e.IdPhatheon)
                .HasMaxLength(50)
                .HasColumnName("ID_Phatheon");
            entity.Property(e => e.IdBangbooDb)
                .HasMaxLength(50)
                .HasColumnName("IDBangboo_DB");

            entity.HasOne(d => d.IdOrderNavigation).WithMany(p => p.CartBoos)
                .HasForeignKey(d => d.IdOrder)
                .HasConstraintName("FK_CartBoo_Boorder");

            entity.HasOne(d => d.IdPhatheonNavigation).WithMany(p => p.CartBoos)
                .HasForeignKey(d => d.IdPhatheon)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CartBoo_Phaethon_User");

            entity.HasOne(d => d.IdbangbooDbNavigation).WithMany(p => p.CartBoos)
                .HasForeignKey(d => d.IdBangbooDb)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CartBoo_Bangboo_DB");
        });

        modelBuilder.Entity<PhaethonUser>(entity =>
        {
            entity.HasKey(e => e.IdPhaethon);

            entity.ToTable("Phaethon_User");

            entity.Property(e => e.IdPhaethon)
                .HasMaxLength(50)
                .HasColumnName("ID_Phaethon");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Create_at");
            entity.Property(e => e.FullNamePhatheon).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
