using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace shopping.Models
{
    public partial class shoppingContext : DbContext
    {
        public shoppingContext()
        {
        }

        public shoppingContext(DbContextOptions<shoppingContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CommentMer> CommentMers { get; set; } = null!;
        public virtual DbSet<CommentProduct> CommentProducts { get; set; } = null!;
        public virtual DbSet<Member> Members { get; set; } = null!;
        public virtual DbSet<Merchant> Merchants { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderDaitel> OrderDaitels { get; set; } = null!;
        public virtual DbSet<Prodect> Prodects { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source = SQL8004.site4now.net; Initial Catalog = db_a96138_project; User Id = db_a96138_project_admin; Password = ahmed123@");

                //optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=shopping;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CommentMer>(entity =>
            {
                entity.HasKey(e => new { e.IdMer, e.IdMember });

                entity.HasOne(d => d.IdMemberNavigation)
                    .WithMany(p => p.CommentMers)
                    .HasForeignKey(d => d.IdMember)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_mem_Comment_Mer");

                entity.HasOne(d => d.IdMerNavigation)
                    .WithMany(p => p.CommentMers)
                    .HasForeignKey(d => d.IdMer)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_mer_Comment_Mer");
            });

            modelBuilder.Entity<CommentProduct>(entity =>
            {
                entity.HasKey(e => new { e.IdPro, e.IdMember });

                entity.HasOne(d => d.IdMemberNavigation)
                    .WithMany(p => p.CommentProducts)
                    .HasForeignKey(d => d.IdMember)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_mem_Comment_product");

                entity.HasOne(d => d.IdProNavigation)
                    .WithMany(p => p.CommentProducts)
                    .HasForeignKey(d => d.IdPro)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Pro_Comment_product");
            });

            modelBuilder.Entity<Member>(entity =>
            {
                entity.Property(e => e.JoinDate).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.TypeUser).HasDefaultValueSql("((100))");
            });

            modelBuilder.Entity<Merchant>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Earning).HasDefaultValueSql("((0.03))");

                entity.Property(e => e.IsActvity).HasDefaultValueSql("((0))");

                entity.Property(e => e.Tax).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Merchant)
                    .HasForeignKey<Merchant>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Merchants");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.DateOrder).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.IsDelivery).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<OrderDaitel>(entity =>
            {
                entity.HasKey(e => new { e.IdOrder, e.IdPro });

                entity.HasOne(d => d.IdOrderNavigation)
                    .WithMany(p => p.OrderDaitels)
                    .HasForeignKey(d => d.IdOrder)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_order_orderDaitels");

                entity.HasOne(d => d.IdProNavigation)
                    .WithMany(p => p.OrderDaitels)
                    .HasForeignKey(d => d.IdPro)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_prodect_orderDaitels");
            });

            modelBuilder.Entity<Prodect>(entity =>
            {
                entity.Property(e => e.IsPublishing).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.IdMerNavigation)
                    .WithMany(p => p.Prodects)
                    .HasForeignKey(d => d.IdMer)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_mer_prodects");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasMany(d => d.IdMs)
                    .WithMany(p => p.IdRs)
                    .UsingEntity<Dictionary<string, object>>(
                        "RolesM",
                        l => l.HasOne<Member>().WithMany().HasForeignKey("IdM").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_IdM_roles"),
                        r => r.HasOne<Role>().WithMany().HasForeignKey("IdR").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Idr_roles"),
                        j =>
                        {
                            j.HasKey("IdR", "IdM").HasName("PK_roles");

                            j.ToTable("RolesM");
                        });
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
