using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ASM.Models
{
    public partial class DMMContext : DbContext
    {
        public DMMContext()
        {
        }

        public DMMContext(DbContextOptions<DMMContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Oder> Oders { get; set; }
        public virtual DbSet<OderDetail> OderDetails { get; set; }
        public virtual DbSet<Page> Pages { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductImage> ProductImages { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Shipper> Shippers { get; set; }
        public virtual DbSet<TransactStatus> TransactStatuses { get; set; }
        public virtual DbSet<Voucher> Vouchers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-56CMKMS;Initial Catalog=PRO1_Full;Persist Security Info=True;User ID=Linhtnph20247;Password=25102003");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Vietnamese_CI_AS");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(150);

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastLogin).HasColumnType("datetime");

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.Salt)
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_Accounts_Roles");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CateId);

                entity.ToTable("Category");

                entity.Property(e => e.CateId).HasColumnName("CateID");

                entity.Property(e => e.Alias).HasMaxLength(255);

                entity.Property(e => e.CateName).HasMaxLength(255);

                entity.Property(e => e.Cover).HasMaxLength(255);

                entity.Property(e => e.Icon).HasMaxLength(255);

                entity.Property(e => e.Img).HasMaxLength(255);

                entity.Property(e => e.Title).HasMaxLength(255);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.Adress).HasMaxLength(200);

                entity.Property(e => e.Avatar).HasMaxLength(200);

                entity.Property(e => e.Birthday).HasColumnType("datetime");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(200);

                entity.Property(e => e.FullName).HasMaxLength(200);

                entity.Property(e => e.LastLogin).HasColumnType("datetime");

                entity.Property(e => e.Password).HasMaxLength(200);

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Salt)
                    .HasMaxLength(8)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(200);

                entity.Property(e => e.NameWithType).HasMaxLength(200);

                entity.Property(e => e.PathWithType).HasMaxLength(200);

                entity.Property(e => e.Slug).HasMaxLength(200);

                entity.Property(e => e.Type).HasMaxLength(50);
            });

            modelBuilder.Entity<Oder>(entity =>
            {
                entity.Property(e => e.OderId).HasColumnName("OderID");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.OderDate).HasColumnType("datetime");

                entity.Property(e => e.PaymentDate).HasColumnType("datetime");

                entity.Property(e => e.ShipDate).HasColumnType("datetime");

                entity.Property(e => e.TransactStatusId).HasColumnName("TransactStatusID");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Oders)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_Oders_Customer");

                entity.HasOne(d => d.TransactStatus)
                    .WithMany(p => p.Oders)
                    .HasForeignKey(d => d.TransactStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Oders_TransactStatus");

                entity.HasOne(d => d.Voucher)
                    .WithMany(p => p.Oders)
                    .HasForeignKey(d => d.VoucherId)
                    .HasConstraintName("FK_Oders_Voucher");
            });

            modelBuilder.Entity<OderDetail>(entity =>
            {
                entity.Property(e => e.OderDetailId).HasColumnName("OderDetailID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.OderId).HasColumnName("OderID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.ShipDate).HasColumnType("datetime");

                entity.HasOne(d => d.Oder)
                    .WithMany(p => p.OderDetails)
                    .HasForeignKey(d => d.OderId)
                    .HasConstraintName("FK_OderDetails_Oders");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OderDetails)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_OderDetails_Products");
            });

            modelBuilder.Entity<Page>(entity =>
            {
                entity.Property(e => e.PageId).HasColumnName("PageID");

                entity.Property(e => e.Alias).HasMaxLength(200);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Img).HasMaxLength(200);

                entity.Property(e => e.MetaDesc).HasMaxLength(200);

                entity.Property(e => e.MetaKey).HasMaxLength(200);

                entity.Property(e => e.PageName).HasMaxLength(200);

                entity.Property(e => e.Title).HasMaxLength(200);
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("Post");

                entity.Property(e => e.PostId).HasColumnName("PostID");

                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.Alias).HasMaxLength(255);

                entity.Property(e => e.Author).HasMaxLength(255);

                entity.Property(e => e.CateId).HasColumnName("CateID");

                entity.Property(e => e.CreateData).HasColumnType("datetime");

                entity.Property(e => e.Img).HasMaxLength(255);

                entity.Property(e => e.IsHot).HasColumnName("isHot");

                entity.Property(e => e.IsNewFeed).HasColumnName("isNewFeed");

                entity.Property(e => e.ShortContents).HasMaxLength(255);

                entity.Property(e => e.Title).HasMaxLength(255);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK_Post_Accounts");

                entity.HasOne(d => d.Cate)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.CateId)
                    .HasConstraintName("FK_Post_Category");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.Alias).HasMaxLength(200);

                entity.Property(e => e.CateId).HasColumnName("CateID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Img).HasMaxLength(200);

                entity.Property(e => e.MetaDesc).HasMaxLength(200);

                entity.Property(e => e.MetaKey).HasMaxLength(200);

                entity.Property(e => e.Modified).HasColumnType("datetime");

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Title).HasMaxLength(200);

                entity.HasOne(d => d.Cate)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CateId)
                    .HasConstraintName("FK_Products_Category");
            });

            modelBuilder.Entity<ProductImage>(entity =>
            {
                entity.ToTable("ProductImage");

                entity.Property(e => e.ImageUrl)
                    .IsRequired()
                    .HasColumnName("ImageURL");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductImages)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductImage_Products");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.RoleDescription).HasMaxLength(50);

                entity.Property(e => e.RoleName)
                    .HasMaxLength(50)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Shipper>(entity =>
            {
                entity.Property(e => e.ShipperId).HasColumnName("ShipperID");

                entity.Property(e => e.Company).HasMaxLength(200);

                entity.Property(e => e.Phone)
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.Property(e => e.ShipDate).HasColumnType("datetime");

                entity.Property(e => e.ShipperName).HasMaxLength(200);
            });

            modelBuilder.Entity<TransactStatus>(entity =>
            {
                entity.ToTable("TransactStatus");

                entity.Property(e => e.TransactStatusId).HasColumnName("TransactStatusID");

                entity.Property(e => e.Status).HasMaxLength(50);
            });

            modelBuilder.Entity<Voucher>(entity =>
            {
                entity.ToTable("Voucher");

                entity.Property(e => e.VoucherId).HasColumnName("VoucherID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.ExpirationDate).HasColumnType("datetime");

                entity.Property(e => e.VoucherCode).IsRequired();

                entity.Property(e => e.VoucherName).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
