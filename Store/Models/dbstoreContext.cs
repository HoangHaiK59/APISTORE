using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Store.Models
{
    public partial class dbstoreContext : DbContext
    {
        public dbstoreContext()
        {
        }

        public dbstoreContext(DbContextOptions<dbstoreContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admin> Admin { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Image> Image { get; set; }
        public virtual DbSet<Marketing> Marketing { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<Transaction> Transaction { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DESKTOP-II770VB\\MSSQL;Database=db.store;User Id=sa;Password=123456;Trusted_Connection=True;MultipleActiveResultSets=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.Property(e => e.Fullname)
                    .IsRequired()
                    .HasColumnName("fullname")
                    .HasMaxLength(128);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(32)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasMaxLength(32)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ParentId).HasColumnName("parent_id");
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.Property(e => e.Color)
                    .IsRequired()
                    .HasColumnName("color")
                    .HasColumnType("text")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(128)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ProductId).HasColumnName("productId");

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasColumnName("url")
                    .HasColumnType("text")
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<Marketing>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(128)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Ordinal)
                    .HasColumnName("ordinal")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasColumnName("url")
                    .HasMaxLength(256)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.Amount)
                    .HasColumnName("amount")
                    .HasColumnType("decimal(15, 4)")
                    .HasDefaultValueSql("((0.0000))");

                entity.Property(e => e.Note)
                    .IsRequired()
                    .HasColumnName("note")
                    .HasColumnType("text")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ProductId).HasColumnName("productId");

                entity.Property(e => e.Qty).HasColumnName("qty");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.TranId).HasColumnName("tranId");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.Created).HasColumnName("created");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasColumnType("text")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Discount).HasColumnName("discount");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(128)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Price)
                    .HasColumnName("price")
                    .HasColumnType("decimal(15, 4)")
                    .HasDefaultValueSql("((0.0000))");

                entity.Property(e => e.View).HasColumnName("view");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.Property(e => e.Amount)
                    .HasColumnName("amount")
                    .HasColumnType("decimal(15, 4)")
                    .HasDefaultValueSql("((0.0000))");

                entity.Property(e => e.Created).HasColumnName("created");

                entity.Property(e => e.CustId)
                    .IsRequired()
                    .HasColumnName("custId")
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasColumnName("message")
                    .HasColumnType("text")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Payment)
                    .IsRequired()
                    .HasColumnName("payment")
                    .HasMaxLength(128)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.PaymentInfo)
                    .IsRequired()
                    .HasColumnName("payment_info")
                    .HasColumnType("text")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Security)
                    .IsRequired()
                    .HasColumnName("security")
                    .HasMaxLength(20)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UserEmail)
                    .IsRequired()
                    .HasColumnName("user_email")
                    .HasMaxLength(128)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnName("user_name")
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.UserPhone)
                    .IsRequired()
                    .HasColumnName("user_phone")
                    .HasMaxLength(15)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasColumnName("address")
                    .HasMaxLength(128)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Created).HasColumnName("created");

                entity.Property(e => e.CustId)
                    .IsRequired()
                    .HasColumnName("custId")
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Fullname)
                    .IsRequired()
                    .HasColumnName("fullname")
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(32)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasColumnName("phone")
                    .HasMaxLength(15)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasMaxLength(32)
                    .HasDefaultValueSql("('')");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
