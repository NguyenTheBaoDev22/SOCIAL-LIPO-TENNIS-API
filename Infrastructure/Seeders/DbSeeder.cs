using Core.Entities;
using Core.Entities.AppUsers;
using Core.Enumerables;
using Infrastructure.Extensions;
using Infrastructure.Seeders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistences
{
    public static class DbSeeder
    {
        //public static async Task SeedAsync(AppDbContext context)
        //{
        //    // 1. Đảm bảo database đã migrate
        //    await context.Database.MigrateAsync();

        //    //// 1.1 Đảm bảo các hàm PostgreSQL custom (nếu có)
        //    await context.EnsurePostgresFunctionsAsync(); // 👈 nếu bạn đã viết extension này

        //    // 2. Seed ClientCredential mặc định
        //    if (!await context.ClientCredentials.AnyAsync(x => x.ClientId == "admin"))
        //    {
        //        var client = new ClientCredential
        //        {
        //            ClientId = "admin",
        //            ClientSecretHash = BCrypt.Net.BCrypt.HashPassword("supersecret"),
        //            Description = "Admin backend service",
        //            IsActive = true,
        //            Role = RoleEnum.AdminDashboard,
        //            CreatedAt = DateTime.UtcNow,
        //            UpdatedAt = DateTime.UtcNow,
        //        };

        //        context.ClientCredentials.Add(client);
        //        await context.SaveChangesAsync();
        //    }

        //    // 2.1. Seed thêm Client "Rồng Việt" nếu chưa có
        //    if (!await context.ClientCredentials.AnyAsync(x => x.ClientId == "RONGVIET"))
        //    {
        //        var partnerClient = new ClientCredential
        //        {
        //            ClientId = "RONGVIET",
        //            ClientSecretHash = BCrypt.Net.BCrypt.HashPassword("Jv7tDKbRZI78MYPmump4rG4H1iy0bQTP"),
        //            Description = "Rồng Việt Team - Anh Việt",
        //            IsActive = true,
        //            Role = RoleEnum.MobileShopManager,
        //            CreatedAt = DateTime.UtcNow,
        //            UpdatedAt = DateTime.UtcNow,
        //        };

        //        context.ClientCredentials.Add(partnerClient);
        //        await context.SaveChangesAsync();
        //    }

        //    // 3. Seed Roles
        //    if (!await context.Roles.AnyAsync())
        //    {
        //        var roles = RoleSeeder.GetDefaultRoles();
        //        context.Roles.AddRange(roles);
        //        await context.SaveChangesAsync();
        //    }

        //    // 4. Seed Permissions
        //    if (!await context.Permissions.AnyAsync())
        //    {
        //        var permissions = PermissionSeeder.GetDefaultPermissions();
        //        context.Permissions.AddRange(permissions);
        //        await context.SaveChangesAsync();
        //    }

        //    // 5. Seed RolePermissions
        //    if (!await context.RolePermissions.AnyAsync())
        //    {
        //        var rolePerms = RolePermissionSeeder.GetDefaultRolePermissions();
        //        context.RolePermissions.AddRange(rolePerms);
        //        await context.SaveChangesAsync();
        //    }

        //    // 6. Seed admin User và gán quyền
        //    if (!await context.Users.AnyAsync(u => u.Username == "admin"))
        //    {
        //        var adminUser = new User
        //        {
        //            Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), // 👈 Gán ID cố định
        //            Username = "admin",
        //            PhoneNumber = "0937127023",
        //            PasswordHash = BCrypt.Net.BCrypt.HashPassword("supersecret"),
        //            IsActive = true,
        //            CreatedAt = DateTime.UtcNow
        //        };

        //        context.Users.Add(adminUser);
        //        await context.SaveChangesAsync();

        //        // 👇 Optional: Gán role nếu bạn dùng multi-tenant thật
        //        var adminRoleId = RoleSeeder.RoleMap["Admin"].Id;
        //        context.UserRoleAssignments.Add(new UserRoleAssignment
        //        {
        //            Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
        //            UserId = adminUser.Id,
        //            RoleId = adminRoleId,
        //            TenantId = null, // hoặc null nếu bạn làm nullable
        //            MerchantId = null,     // ✅ Cho phép null khi MerchantId nullable
        //            CreatedAt = DateTime.UtcNow
        //        });
        //        await context.SaveChangesAsync();
        //    }

        //    // ✅ Done
        //}
    }
}
