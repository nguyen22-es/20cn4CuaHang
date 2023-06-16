using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CuaHangCongNghe.Models.Tables
{
    public partial class storeContext : DbContext
    {
        public storeContext()
        {
        }

        public storeContext(DbContextOptions<storeContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Dangnhapuser> Dangnhapusers { get; set; } = null!;
        public virtual DbSet<Namerole> Nameroles { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<Orderitem> Orderitems { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=localhost;database=store;user=ducanh;password=16102002;allow user variables=True", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.33-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("categories");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .UseCollation("utf8mb4_unicode_ci");
            });

            modelBuilder.Entity<Dangnhapuser>(entity =>
            {
                entity.HasKey(e => e.Iddangnhap)
                    .HasName("PRIMARY");

                entity.ToTable("dangnhapuser");

                entity.HasIndex(e => e.Idrole, "FK_dangnhapuser_namerole");

                entity.Property(e => e.Iddangnhap).HasColumnName("iddangnhap");

                entity.Property(e => e.Idrole).HasColumnName("idrole");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .HasColumnName("password");

                entity.Property(e => e.Tendangnhap)
                    .HasMaxLength(50)
                    .HasColumnName("tendangnhap");

                entity.HasOne(d => d.IdroleNavigation)
                    .WithMany(p => p.Dangnhapusers)
                    .HasForeignKey(d => d.Idrole)
                    .HasConstraintName("FK_dangnhapuser_namerole");
            });

            modelBuilder.Entity<Namerole>(entity =>
            {
                entity.HasKey(e => e.Idrole)
                    .HasName("PRIMARY");

                entity.ToTable("namerole");

                entity.Property(e => e.Idrole)
                    .ValueGeneratedNever()
                    .HasColumnName("idrole");

                entity.Property(e => e.Tenrole)
                    .HasMaxLength(50)
                    .HasColumnName("tenrole");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("orders");

                entity.HasIndex(e => e.UserId, "orders_ibfk_1");

                entity.Property(e => e.OrderId)
                    .ValueGeneratedNever()
                    .HasColumnName("order_id");

                entity.Property(e => e.OrderDate).HasColumnName("order_date");

                entity.Property(e => e.Status)
                    .HasMaxLength(255)
                    .HasColumnName("status")
                    .UseCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("orders_ibfk_1");
            });

            modelBuilder.Entity<Orderitem>(entity =>
            {
                entity.HasKey(e => e.OrderItemsId)
                    .HasName("PRIMARY");

                entity.ToTable("orderitems");

                entity.HasIndex(e => e.OrderId, "orderitems_ibfk_1");

                entity.HasIndex(e => e.ProductId, "product_id");

                entity.Property(e => e.OrderItemsId)
                    .ValueGeneratedNever()
                    .HasColumnName("order_items_id");

                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Orderitems)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("orderitems_ibfk_1");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Orderitems)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("orderitems_ibfk_2");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("products");

                entity.HasIndex(e => e.CategoryId, "FK_products_categories");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CategoryId).HasColumnName("categoryID");

                entity.Property(e => e.Description)
                    .HasColumnType("text")
                    .UseCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(255)
                    .HasColumnName("ImageURL");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .UseCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.Stockquantity).HasColumnName("stockquantity");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_products_categories");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasIndex(e => e.Iddangnhap, "FK_users_dangnhapuser");

                entity.HasIndex(e => e.Idrole, "FK_users_namerole");

                entity.Property(e => e.UserId)
                    .ValueGeneratedNever()
                    .HasColumnName("user_id");

                entity.Property(e => e.AddressUser)
                    .HasMaxLength(255)
                    .HasColumnName("address_user")
                    .UseCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.EmailUser)
                    .HasMaxLength(255)
                    .HasColumnName("email_user")
                    .UseCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Iddangnhap).HasColumnName("iddangnhap");

                entity.Property(e => e.Idrole).HasColumnName("idrole");

                entity.Property(e => e.NameUser)
                    .HasMaxLength(200)
                    .HasColumnName("name_user")
                    .UseCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.PhoneUser)
                    .HasMaxLength(20)
                    .HasColumnName("phone_user");

                entity.Property(e => e.RegistrationDate).HasColumnName("registration_date");

                entity.HasOne(d => d.IddangnhapNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.Iddangnhap)
                    .HasConstraintName("FK_users_dangnhapuser");

                entity.HasOne(d => d.IdroleNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.Idrole)
                    .HasConstraintName("FK_users_namerole");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
