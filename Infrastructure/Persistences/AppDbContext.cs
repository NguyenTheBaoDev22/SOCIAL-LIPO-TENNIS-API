using Core;
using Core.Entities;
using Core.Entities.AppUsers;
using Core.Entities.Integrations;
using Core.Entities.Logs;
using Core.Entities.Partners;
using Core.Entities.Shops;
using Core.Entities.Tenants;
using Infrastructure.Persistences.Interceptors;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shared.Interfaces;
using System.Collections.Concurrent;
using System.Reflection;

namespace Infrastructure.Persistences
{
    public class AppDbContext : DbContext
    {
        private readonly ICurrentUserService _currentUser;
        private readonly AuditSaveChangesInterceptor _auditInterceptor;
        private readonly AuditLogInterceptor _auditLogInterceptor;
        private static readonly ConcurrentDictionary<Type, MethodInfo> _softDeleteMethodCache = new();
        public AppDbContext(
            DbContextOptions<AppDbContext> options,
            ICurrentUserService currentUser,
            AuditSaveChangesInterceptor auditInterceptor,
            AuditLogInterceptor auditLogInterceptor)
            : base(options)
        {
            _currentUser = currentUser;
            _auditInterceptor = auditInterceptor;
            _auditLogInterceptor = auditLogInterceptor;
        }

        // DbSets
        public DbSet<ClientCredential> ClientCredentials => Set<ClientCredential>();
        public DbSet<SmsLog> SmsLogs => Set<SmsLog>();
        public DbSet<SmsRetryQueue> SmsRetryQueues => Set<SmsRetryQueue>();
        public DbSet<Province> Provinces => Set<Province>();
        public DbSet<Commune> Communes => Set<Commune>();
        public DbSet<Merchant> Merchants => Set<Merchant>();
        public DbSet<MerchantBranch> MerchantBranches => Set<MerchantBranch>();
        public DbSet<PaymentTerminal> PaymentTerminals => Set<PaymentTerminal>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Permission> Permissions => Set<Permission>();
        public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
        public DbSet<UserRoleAssignment> UserRoleAssignments => Set<UserRoleAssignment>();
        public DbSet<UserMerchant> UserMerchants => Set<UserMerchant>();
        public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<ProductImport> ProductImports => Set<ProductImport>();
        public DbSet<ShopProductInventory> ShopProductInventories => Set<ShopProductInventory>();
        public DbSet<PartnerTransactionCallbackLog> PartnerTransactionCallbackLogs => Set<PartnerTransactionCallbackLog>();
        public DbSet<PartnerMerchantStatusCallbackLog> PartnerMerchantStatusCallbackLogs => Set<PartnerMerchantStatusCallbackLog>();
        public DbSet<PartnerOrder> PartnerOrders => Set<PartnerOrder>();
        public DbSet<ZaloAuthLog> ZaloAuthLogs => Set<ZaloAuthLog>();
        public DbSet<LarkEmailLog> LarkEmailLogs => Set<LarkEmailLog>();
        public DbSet<OtpCodeLog> OtpCodeLogs => Set<OtpCodeLog>();
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
        public DbSet<OtpRequestCounter> OtpRequestCounters { get; set; } = default!;
        public DbSet<LarkTokens> LarkTokens { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var tenantId = _currentUser.TenantId;
            modelBuilder.Entity<AuditLog>().HasKey(a => a.Id);
            modelBuilder.Entity<AuditLog>().Property(a => a.EntityName).IsRequired().HasMaxLength(255);
            modelBuilder.Entity<AuditLog>().Property(a => a.EntityId).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<AuditLog>().Property(a => a.ActionType).IsRequired().HasMaxLength(20);
            modelBuilder.Entity<AuditLog>().Property(a => a.Changes).HasColumnType("jsonb"); // nếu dùng PostgreSQL
            // Lọc dữ liệu của từng tenant
            modelBuilder.Entity<Merchant>().HasQueryFilter(x => x.TenantId == tenantId && !x.IsDeleted);
            modelBuilder.Entity<MerchantBranch>().HasQueryFilter(x => x.Merchant.TenantId == tenantId && !x.IsDeleted);
            modelBuilder.Entity<PaymentTerminal>().HasQueryFilter(x => x.Merchant.TenantId == tenantId && x.MerchantBranch.TenantId == tenantId && !x.IsDeleted);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(Audit).IsAssignableFrom(entityType.ClrType))
                {
                    var clrType = entityType.ClrType;

                    var genericMethod = _softDeleteMethodCache.GetOrAdd(clrType, t =>
                    {
                        var method = typeof(AppDbContext)
                            .GetMethod(nameof(SetSoftDeleteFilter), BindingFlags.NonPublic | BindingFlags.Instance);
                        return method!.MakeGenericMethod(t);
                    });

                    genericMethod.Invoke(this, new object[] { modelBuilder });
                }
            }

            base.OnModelCreating(modelBuilder);

            // Cấu hình từng entity — giữ nguyên như bạn đã viết, không thay đổi logic
            ConfigureEntities(modelBuilder);
        }

        private void ConfigureEntities(ModelBuilder modelBuilder)
        {
            // Viết đúng như đoạn bạn đã có — không lặp lại ở đây cho ngắn gọn
            // Copy/paste phần: Province, Commune, Merchant, MerchantBranch, User, Role, Permission, Product, v.v.
            // Bạn đã đúng hoàn toàn, giữ nguyên.
            //Province
            
            // Cấu hình các entity (bạn đã chuẩn rất tốt phần này rồi, giữ nguyên):
            modelBuilder.Entity<Province>().HasKey(p => p.Id);

            modelBuilder.Entity<Province>().Property(p => p.Code).IsRequired().HasMaxLength(10);
            modelBuilder.Entity<Province>().Property(p => p.Name).IsRequired().HasMaxLength(100);

            modelBuilder.Entity<Commune>().HasKey(c => c.Id);
            modelBuilder.Entity<Commune>().Property(c => c.Code).IsRequired().HasMaxLength(10);
            modelBuilder.Entity<Commune>().Property(c => c.Name).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Commune>().HasOne(c => c.Province).WithMany(p => p.Communes).HasForeignKey(c => c.ProvinceId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Merchant>().HasKey(e => e.Id);
            modelBuilder.Entity<Merchant>().Property(e => e.SequenceNumber).ValueGeneratedOnAdd();
            modelBuilder.Entity<Merchant>().Property(e => e.MerchantCode).HasComputedColumnSql("ufn_generate_merchant_code(\"SequenceNumber\")", stored: true);
            modelBuilder.Entity<Merchant>().HasIndex(m => m.BusinessRegistrationNo).IsUnique();
            modelBuilder.Entity<Merchant>().HasIndex(m => m.MerchantCode).IsUnique();

            modelBuilder.Entity<MerchantBranch>().HasKey(e => e.Id);
            modelBuilder.Entity<MerchantBranch>().Property(e => e.SequenceNumber).ValueGeneratedOnAdd();
            modelBuilder.Entity<MerchantBranch>().Property(e => e.MerchantBranchCode).HasComputedColumnSql("ufn_generate_merchant_branch_code(\"MerchantId\", \"SequenceNumber\")", stored: true);
            modelBuilder.Entity<MerchantBranch>().HasIndex(mb => new { mb.MerchantCode, mb.MerchantBranchCode }).IsUnique();

            modelBuilder.Entity<PaymentTerminal>().HasIndex(pt => new { pt.MerchantCode, pt.MerchantBranchCode, pt.TerminalCode }).IsUnique();

            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();

            modelBuilder.Entity<Role>().HasKey(r => r.Id);
            modelBuilder.Entity<Role>().HasIndex(r => r.Code).IsUnique();

            modelBuilder.Entity<Permission>().HasKey(p => p.Id);
            modelBuilder.Entity<Permission>().HasIndex(p => p.Code).IsUnique();

            modelBuilder.Entity<RolePermission>().HasKey(rp => new { rp.RoleId, rp.PermissionId });
            modelBuilder.Entity<RolePermission>().HasOne(rp => rp.Role).WithMany(r => r.RolePermissions).HasForeignKey(rp => rp.RoleId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<RolePermission>().HasOne(rp => rp.Permission).WithMany().HasForeignKey(rp => rp.PermissionId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<RolePermission>().HasIndex(rp => new { rp.RoleId, rp.PermissionId }).IsUnique();

            modelBuilder.Entity<UserRoleAssignment>().HasKey(ua => ua.Id);
            modelBuilder.Entity<UserRoleAssignment>().HasOne(ua => ua.User).WithMany(u => u.UserRoleAssignments).HasForeignKey(ua => ua.UserId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<UserRoleAssignment>().HasOne(ua => ua.Role).WithMany().HasForeignKey(ua => ua.RoleId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<UserRoleAssignment>().HasOne(ua => ua.Merchant).WithMany().HasForeignKey(ua => ua.MerchantId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<UserRoleAssignment>().HasOne(ua => ua.MerchantBranch).WithMany().HasForeignKey(ua => ua.MerchantBranchId).OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<UserRoleAssignment>().HasIndex(ua => new { ua.UserId, ua.RoleId, ua.MerchantId, ua.MerchantBranchId }).IsUnique();

            modelBuilder.Entity<ProductCategory>().HasKey(pc => pc.Id);
            modelBuilder.Entity<ProductCategory>().Property(pc => pc.Code).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<ProductCategory>().Property(pc => pc.Name).IsRequired().HasMaxLength(255);
            modelBuilder.Entity<ProductCategory>().HasIndex(pc => new { pc.TenantId, pc.MerchantId, pc.MerchantBranchId }).IsUnique();

            modelBuilder.Entity<Product>().HasKey(p => p.Id);
            modelBuilder.Entity<Product>().HasIndex(p => p.ProductCode).IsUnique();
            modelBuilder.Entity<Product>().HasOne(p => p.ProductCategory).WithMany(pc => pc.Products).HasForeignKey(p => p.ProductCategoryId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductImport>().HasKey(pi => pi.Id);
            modelBuilder.Entity<ProductImport>().HasOne(pi => pi.Product).WithMany(p => p.ProductImports).HasForeignKey(pi => pi.ProductId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ShopProductInventory>().HasKey(ia => ia.Id);
            modelBuilder.Entity<ShopProductInventory>().HasOne(ia => ia.Product).WithMany(p => p.InventoryAudits).HasForeignKey(ia => ia.ProductId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PartnerOrder>().Property(p => p.Status).HasConversion<string>();
            modelBuilder.Entity<PartnerOrder>().HasIndex(p => new { p.PartnerCode, p.OrderCode });

            modelBuilder.Entity<PartnerMerchantStatusCallbackLog>().HasKey(p => p.Id);
            modelBuilder.Entity<PartnerMerchantStatusCallbackLog>().Property(p => p.Status).HasConversion<string>().HasMaxLength(50);
            modelBuilder.Entity<PartnerMerchantStatusCallbackLog>().Property(p => p.CallbackUrl).IsRequired().HasMaxLength(500);
            modelBuilder.Entity<PartnerMerchantStatusCallbackLog>().Property(p => p.Payload).IsRequired();
            modelBuilder.Entity<PartnerMerchantStatusCallbackLog>().Property(p => p.ResponseContent).HasMaxLength(5000);

            modelBuilder.Entity<PartnerTransactionCallbackLog>().HasKey(p => p.Id);
            modelBuilder.Entity<PartnerTransactionCallbackLog>().Property(p => p.Status).HasConversion<string>().HasMaxLength(50);
            modelBuilder.Entity<PartnerTransactionCallbackLog>().Property(p => p.CallbackUrl).IsRequired().HasMaxLength(500);
            modelBuilder.Entity<PartnerTransactionCallbackLog>().Property(p => p.Payload).IsRequired();
            modelBuilder.Entity<PartnerTransactionCallbackLog>().Property(p => p.ResponseContent).HasMaxLength(5000);
            modelBuilder.Entity<PartnerTransactionCallbackLog>().HasIndex(p => p.PartnerOrderId);

            modelBuilder.Entity<ZaloAuthLog>().HasKey(z => z.Id);
            modelBuilder.Entity<ZaloAuthLog>().Property(z => z.TraceId).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<ZaloAuthLog>().Property(z => z.RequestUrl).IsRequired().HasMaxLength(500);
            modelBuilder.Entity<ZaloAuthLog>().Property(z => z.Token).IsRequired().HasMaxLength(200);
            modelBuilder.Entity<ZaloAuthLog>().Property(z => z.PhoneNumber).HasMaxLength(20);
            modelBuilder.Entity<ZaloAuthLog>().Property(z => z.ErrorMessage).HasMaxLength(1000);

            modelBuilder.Entity<LarkEmailLog>().HasKey(e => e.Id);
            modelBuilder.Entity<LarkEmailLog>().Property(e => e.Subject).IsRequired().HasMaxLength(500);
            modelBuilder.Entity<LarkEmailLog>().Property(e => e.Body).IsRequired();
            modelBuilder.Entity<LarkEmailLog>().Property(e => e.Recipient).IsRequired().HasMaxLength(255);
            modelBuilder.Entity<LarkEmailLog>().Property(e => e.From).IsRequired().HasMaxLength(255);
            modelBuilder.Entity<LarkEmailLog>().Property(e => e.Status).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<OtpCodeLog>(entity =>
            {
                entity.HasIndex(x => new { x.PhoneNumber, x.Purpose, x.IsVerified, x.IsDeleted, x.ExpireAt, x.CreatedAt })
                      .HasDatabaseName("IX_OtpCodeLogs_Phone_Purpose_Expire");
            });
            modelBuilder.Entity<OtpRequestCounter>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => new { x.PhoneNumber, x.Purpose }).IsUnique(false);
                entity.Property(x => x.PhoneNumber).IsRequired().HasMaxLength(20);
                entity.Property(x => x.Purpose).IsRequired().HasMaxLength(50);
                entity.Property(x => x.Count).HasDefaultValue(0);
            });
        }

        private void SetSoftDeleteFilter<TEntity>(ModelBuilder builder) where TEntity : Audit
        {
            if (typeof(TenantEntity).IsAssignableFrom(typeof(TEntity)))
            {
                builder.Entity<TEntity>().HasQueryFilter(e =>
                    !e.IsDeleted && ((TenantEntity)(object)e).TenantId == _currentUser.TenantId);
            }
            else
            {
                builder.Entity<TEntity>().HasQueryFilter(e => !e.IsDeleted);
            }
        }


        private void ApplyAuditInfo()
        {
            var userId = _currentUser.UserId;
            var traceId = _currentUser.TraceId;
            var tenantId = _currentUser.TenantId;
            var now = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries<Audit>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = now;
                        entry.Entity.CreatedBy ??= userId;
                        entry.Entity.TraceId = traceId;
                        entry.Entity.IsDeleted = false;
                        break;

                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = now;
                        entry.Entity.UpdatedBy = userId;
                        entry.Entity.TraceId = traceId;
                        break;

                    case EntityState.Deleted:
                        // Soft-delete logic
                        entry.State = EntityState.Modified;
                        entry.Entity.IsDeleted = true;
                        entry.Entity.UpdatedAt = now;
                        entry.Entity.UpdatedBy = userId;
                        entry.Entity.TraceId = traceId;
                        break;
                }
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(_auditInterceptor, _auditLogInterceptor);

        }
    }
}
