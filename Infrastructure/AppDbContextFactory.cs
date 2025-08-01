using Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Shared.Interfaces;
using Moq;
using Infrastructure.Persistences.Interceptors;

namespace Infrastructure
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var basePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "API"));

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));

            // ✅ Fake ICurrentUserService để phục vụ cho design-time (EF Migrations)
            var currentUser = new FakeCurrentUserService();
            var auditInterceptor = new AuditSaveChangesInterceptor(currentUser);
            var auditLogInterceptor = new AuditLogInterceptor(currentUser);
            return new AppDbContext(
                optionsBuilder.Options,
                currentUser,
                auditInterceptor,
                 auditLogInterceptor
            );
        }
    }


    /// <summary>
    /// FakeCurrentUserService dùng để chạy EF Migrations tại Design-Time
    /// </summary>

    public class FakeCurrentUserService : ICurrentUserService
    {
        public Guid? UserId => Guid.Parse("00000000-0000-0000-0000-000000000001");
        public string? PhoneNumber => "0123456789";
        public string? Email => "admin@zenshop.vn";
        public Guid? TenantId => Guid.Parse("11111111-1111-1111-1111-111111111111");
        public Guid? MerchantId => Guid.Parse("22222222-2222-2222-2222-222222222222");
        public Guid? MerchantBranchId => Guid.Parse("33333333-3333-3333-3333-333333333333");
        public List<string> Roles => new() { "Admin" };
        public List<string> Permissions => new() { "AllAccess" };
        public string? TraceId => "migration-trace";
        public string? IpAddress => "127.0.0.1";
        public string? UserAgent => "EFCore-DesignTime";
        public bool IsAuthenticated => true;
        public void SetTraceId(string traceId) { }
    }

}
