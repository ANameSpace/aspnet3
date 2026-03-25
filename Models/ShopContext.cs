using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PSite3.Models;

public partial class ShopContext : DbContext
{
    public ShopContext()
    {
    }

    public ShopContext(DbContextOptions<ShopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.IdCart).HasName("PK__Carts__70179490BE72049F");

            entity.Property(e => e.IdCart).HasColumnName("ID_cart");
            entity.Property(e => e.UserId).HasColumnName("user_ID");

            entity.HasOne(d => d.User).WithMany(p => p.Carts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Carts__user_ID__30F848ED");
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.IdCartItem).HasName("PK__CartItem__2A8716471AAA2E54");

            entity.HasIndex(e => new { e.CartId, e.ProductId }, "UQ__CartItem__EA8CD9814CDEE49F").IsUnique();

            entity.Property(e => e.IdCartItem).HasColumnName("ID_cartItem");
            entity.Property(e => e.CartId).HasColumnName("cart_ID");
            entity.Property(e => e.ProductId).HasColumnName("product_ID");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Cart).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.CartId)
                .HasConstraintName("FK__CartItems__cart___35BCFE0A");

            entity.HasOne(d => d.Product).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__CartItems__produ__36B12243");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.IdCategory).HasName("PK__Categori__AC5CA40E859358CB");

            entity.Property(e => e.IdCategory).HasColumnName("ID_category");
            entity.Property(e => e.Name)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.IdOrder).HasName("PK__Orders__89BFCD593BAB65A7");

            entity.Property(e => e.IdOrder).HasColumnName("ID_order");
            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("orderDate");
            entity.Property(e => e.UserId).HasColumnName("user_ID");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__user_ID__3A81B327");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.IdCartItem).HasName("PK__OrderIte__2A871647DF1F8C3B");

            entity.HasIndex(e => new { e.OrderId, e.ProductId }, "UQ__OrderIte__823672BF4D0385E5").IsUnique();

            entity.Property(e => e.IdCartItem).HasColumnName("ID_cartItem");
            entity.Property(e => e.OrderId).HasColumnName("order_ID");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.ProductId).HasColumnName("product_ID");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__OrderItem__order__403A8C7D");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderItem__produ__412EB0B6");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.IdProduct).HasName("PK__Products__FD7FEC2306487B88");

            entity.Property(e => e.IdProduct).HasColumnName("ID_product");
            entity.Property(e => e.CategoryId).HasColumnName("category_ID");
            entity.Property(e => e.Description)
                .HasMaxLength(512)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Products__catego__2E1BDC42");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRole).HasName("PK__Roles__45DFFBDBE556C088");

            entity.HasIndex(e => e.Name, "UQ__Roles__72E12F1BE3EA156A").IsUnique();

            entity.Property(e => e.IdRole).HasColumnName("ID_role");
            entity.Property(e => e.Name)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("PK__Users__D7B4671E66CD4759");

            entity.HasIndex(e => e.Login, "UQ__Users__7838F27215F9E06A").IsUnique();

            entity.Property(e => e.IdUser).HasColumnName("ID_user");
            entity.Property(e => e.Hash)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("hash");
            entity.Property(e => e.Login)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("login");
            entity.Property(e => e.RoleId).HasColumnName("role_ID");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Users__role_ID__286302EC");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
