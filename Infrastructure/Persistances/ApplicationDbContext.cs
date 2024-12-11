using Domain.Entities;
using Domain.EntityBase;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistances
{
    public class ApplicationDbContext : DbContext
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly int? UserId = null;

        #region User 
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        #endregion

        #region Product 
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<UserCustomer> UserCustomers { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        #endregion

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            var claims = _httpContextAccessor.HttpContext?.User?.Identity as ClaimsIdentity;
            var claim = claims?.FindFirst("user_id");
            if (claim != null && int.TryParse(claim.Value, out int userId))
            {
                UserId = userId;
            }
        }

        private void CheckAudit()
        {
            ChangeTracker.DetectChanges();
            var added = ChangeTracker.Entries().Where(t => t.State == EntityState.Added).Select(t => t.Entity).AsParallel();
            added.ForAll(entity =>
            {
                if (entity is ICreatedBy createdEntity && createdEntity.CreatedBy == null)
                {
                    createdEntity.CreatedDate = DateTime.Now;
                    createdEntity.CreatedBy = UserId;
                }
            });

            var modified = ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).Select(t => t.Entity).AsParallel();
            modified.ForAll(entity =>
            {
                if (entity is IModifiedBy modifiedEntity && modifiedEntity.ModifiedBy == null)
                {
                    modifiedEntity.ModifiedDate = DateTime.Now;
                    modifiedEntity.ModifiedBy = UserId;
                }
            });
        }

        public override int SaveChanges()
        {
            CheckAudit();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            CheckAudit();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            CheckAudit();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            CheckAudit();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entityTypes = modelBuilder.Model.GetEntityTypes();
            foreach (var entity in entityTypes)
            {
                if (entity.ClrType.IsAssignableTo(typeof(ICreatedBy)))
                {
                    // Custom configuration for audit can be placed here
                }
            }

            #region User
            modelBuilder.Entity<User>(e =>
            {
                e.HasKey(u => u.Id);
                e.Property(u => u.Username).IsRequired().HasMaxLength(50);
                e.Property(u => u.Password).IsRequired().HasMaxLength(512);
                e.HasMany(u => u.UserRoles).WithOne(ur => ur.User).HasForeignKey(ur => ur.UserId);
                e.HasMany(u => u.Carts).WithOne(c => c.User).HasForeignKey(c => c.UserId);
            });
            #endregion

            #region Role
            modelBuilder.Entity<Role>(e =>
            {
                e.HasKey(r => r.Id);
                e.Property(r => r.CreatedDate).HasDefaultValue(DateTime.Now);
                e.Property(r => r.Deleted).HasDefaultValue(false);
                e.HasMany(r => r.UserRoles).WithOne(ur => ur.Role).HasForeignKey(ur => ur.RoleId).OnDelete(DeleteBehavior.Restrict);
            });
            #endregion

            #region Category
            modelBuilder.Entity<Category>(e =>
            {
                e.HasKey(c => c.Id);
                e.Property(c => c.Name).IsRequired().HasMaxLength(100);
            });
            #endregion

            #region Customer
            modelBuilder.Entity<Customer>(e =>
            {
                e.HasKey(c => c.Id);
                e.Property(c => c.FirstName).IsRequired().HasMaxLength(50);
                e.Property(c => c.LastName).IsRequired().HasMaxLength(50);
                e.Property(c => c.Email).IsRequired().HasMaxLength(100);
                e.Property(c => c.Phone).IsRequired().HasMaxLength(20);
                e.Property(c => c.Address).HasMaxLength(255);

                e.Property(c => c.CreatedDate).HasDefaultValueSql("GETDATE()");
                e.Property(c => c.Deleted).HasDefaultValue(false);

            });
            #endregion


            #region Order
            modelBuilder.Entity<Order>(e =>
            {
                e.HasKey(o => o.Id);
                e.Property(o => o.OrderDate).IsRequired();
                e.Property(o => o.TotalAmount).IsRequired().HasColumnType("decimal(10, 2)");
                e.HasOne(o => o.Customer)
                    .WithMany(c => c.Orders)
                    .HasForeignKey(o => o.CustomerId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion

            #region OrderDetail
            modelBuilder.Entity<OrderDetail>(e =>
            {
                e.HasKey(od => od.Id);
                e.Property(od => od.Quantity).IsRequired();
                e.Property(od => od.UnitPrice).IsRequired().HasColumnType("decimal(10, 2)");
                e.Property(od => od.Measurements).IsRequired().HasMaxLength(255);
                e.HasOne(od => od.Order).WithMany(o => o.OrderDetails).HasForeignKey(od => od.OrderId);
                e.HasOne(od => od.Product).WithMany(p => p.OrderDetails).HasForeignKey(od => od.ProductId);
            });
            #endregion

            #region Product
            modelBuilder.Entity<Product>(e =>
            {
                e.HasKey(p => p.Id);
                e.Property(p => p.Name).IsRequired().HasMaxLength(100);
                e.Property(p => p.Description).HasMaxLength(500);
                e.Property(p => p.Price).IsRequired().HasColumnType("decimal(10, 2)");
                e.HasMany(p => p.ProductCategories).WithOne(pc => pc.Product).HasForeignKey(pc => pc.ProductId);
                e.HasMany(p => p.ProductImages).WithOne(pi => pi.Product).HasForeignKey(pi => pi.ProductId);
                e.HasMany(p => p.OrderDetails).WithOne(od => od.Product).HasForeignKey(od => od.ProductId);
            });
            #endregion

            #region ProductCategory
            modelBuilder.Entity<ProductCategory>(e =>
            {
                e.HasKey(pc => pc.Id);
                e.HasOne(pc => pc.Product).WithMany(p => p.ProductCategories).HasForeignKey(pc => pc.ProductId);
                e.HasOne(pc => pc.Category).WithMany(c => c.ProductCategories).HasForeignKey(pc => pc.CategoryId);
            });
            #endregion

            #region ProductImage
            modelBuilder.Entity<ProductImage>(e =>
            {
                e.HasKey(pi => pi.Id);
                e.Property(pi => pi.ImageUrl).IsRequired().HasMaxLength(255);
                e.HasOne(pi => pi.Product).WithMany(p => p.ProductImages).HasForeignKey(pi => pi.ProductId);
            });
            #endregion

            #region PaymentMethod
            modelBuilder.Entity<PaymentMethod>(e =>
            {
                e.HasKey(pm => pm.Id);
                e.Property(pm => pm.Name).IsRequired().HasMaxLength(50);
            });
            #endregion

            #region Payment
            modelBuilder.Entity<Payment>(e =>
            {
                e.HasKey(p => p.ID);
                e.Property(p => p.PaymentDate).IsRequired();
                e.Property(p => p.Amount).IsRequired().HasColumnType("decimal(10, 2)");
                e.HasOne(p => p.PaymentMethod).WithMany(pm => pm.Payments).HasForeignKey(p => p.PaymentMethodID);
                e.HasOne(p => p.Order).WithMany(o => o.Payments).HasForeignKey(p => p.OrderID);
            });
            #endregion

            #region UserCustomer 
            modelBuilder.Entity<UserCustomer>(e => { e.HasKey(uc => uc.ID); 
                e.HasIndex(uc => new { uc.UserID, uc.CustomerID }).IsUnique(); 
                e.HasOne(uc => uc.User) 
                .WithMany(u => u.UserCustomers) 
                .HasForeignKey(uc => uc.UserID); 
                e.HasOne(uc => uc.Customer) 
                .WithMany(c => c.UserCustomers) 
                .HasForeignKey(uc => uc.CustomerID); });
            #endregion

            #region Cart 
            modelBuilder.Entity<Cart>(e => { e.HasKey(c => c.Id); 
                e.HasOne(c => c.User).WithMany(u => u.Carts)
                .HasForeignKey(c => c.UserId); 

                e.HasMany(c => c.Items)
                .WithOne(ci => ci.Cart)
                .HasForeignKey(ci => ci.CartId); }); 
            #endregion

            #region CartItem 
            modelBuilder.Entity<CartItem>(e => { e.HasKey(ci => ci.Id); 
                e.Property(ci => ci.Quantity)
                .IsRequired(); 
                e.HasOne(ci => ci.Product)
                .WithMany()
                .HasForeignKey(ci => ci.ProductId); }); 
            #endregion

            base.OnModelCreating(modelBuilder);

        }
    }
}
