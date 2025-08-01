using Core.Entities.AppUsers;
using Core.Enumerables;
using Shared.Authorization;
using System.Security.Cryptography;

namespace Infrastructure.Seeders
{
    public static class RolePermissionSeeder
    {
        //public static List<RolePermission> GetDefaultRolePermissions()
        //{
        //    var list = new List<RolePermission>();

        //    void Add(string roleCode, PermissionCode permissionCode)
        //    {
        //        var roleId = RoleSeeder.RoleMap[roleCode].Id;
        //        var permissionId = PermissionConstants.Map[permissionCode];

        //        list.Add(new RolePermission
        //        {
        //            Id = GenerateDeterministicGuid(roleId, permissionId),
        //            RoleId = roleId,
        //            PermissionId = permissionId,
        //            CreatedAt = new DateTime(2024, 01, 01)
        //        });
        //    }

        //    static Guid GenerateDeterministicGuid(Guid roleId, Guid permissionId)
        //    {
        //        var bytes = roleId.ToByteArray().Concat(permissionId.ToByteArray()).ToArray();
        //        using var sha1 = SHA1.Create();
        //        var hash = sha1.ComputeHash(bytes);
        //        return new Guid(hash.Take(16).ToArray());
        //    }

        //    // Owner: full permissions
        //    foreach (var perm in PermissionConstants.Map.Keys)
        //        Add("Owner", perm);

        //    // Manager
        //    Add("Manager", PermissionCode.ViewDashboard);
        //    Add("Manager", PermissionCode.UpdateMerchantInfo);
        //    Add("Manager", PermissionCode.AddBranch);
        //    Add("Manager", PermissionCode.UpdateBranch);
        //    Add("Manager", PermissionCode.DeclareDevice);
        //    Add("Manager", PermissionCode.CheckDeviceStatus);
        //    Add("Manager", PermissionCode.CreateOrder);
        //    Add("Manager", PermissionCode.EditOrder);
        //    Add("Manager", PermissionCode.CancelOrder);
        //    Add("Manager", PermissionCode.ViewOrder);
        //    Add("Manager", PermissionCode.ManageInvoice);
        //    Add("Manager", PermissionCode.DraftInvoice);
        //    Add("Manager", PermissionCode.SignInvoice);
        //    Add("Manager", PermissionCode.ExportInvoice);
        //    Add("Manager", PermissionCode.ManageProduct);
        //    Add("Manager", PermissionCode.AddProduct);
        //    Add("Manager", PermissionCode.EditProduct);
        //    Add("Manager", PermissionCode.DeleteProduct);
        //    Add("Manager", PermissionCode.ImportInventory);
        //    Add("Manager", PermissionCode.ManageUsers);
        //    Add("Manager", PermissionCode.CreateUser);
        //    Add("Manager", PermissionCode.AssignRole);
        //    Add("Manager", PermissionCode.ViewUserActivity);
        //    Add("Manager", PermissionCode.ViewReport);
        //    Add("Manager", PermissionCode.ExportReport);
        //    Add("Manager", PermissionCode.ViewTransactionHistory);
        //    Add("Manager", PermissionCode.SearchTransaction);
        //    Add("Manager", PermissionCode.RequestPurchase);
        //    Add("Manager", PermissionCode.FeedbackSubmit);
        //    Add("Manager", PermissionCode.ReportIssue);
        //    Add("Manager", PermissionCode.AccessHotlineSupport);
        //    Add("Manager", PermissionCode.ConfigureSecurityDevice);

        //    // Cashier
        //    Add("Cashier", PermissionCode.ViewDashboard);
        //    Add("Cashier", PermissionCode.CreateOrder);
        //    Add("Cashier", PermissionCode.EditOrder);
        //    Add("Cashier", PermissionCode.CancelOrder);
        //    Add("Cashier", PermissionCode.ViewOrder);
        //    Add("Cashier", PermissionCode.DraftInvoice);
        //    Add("Cashier", PermissionCode.ViewTransactionHistory);
        //    Add("Cashier", PermissionCode.SearchTransaction);
        //    Add("Cashier", PermissionCode.FeedbackSubmit);
        //    Add("Cashier", PermissionCode.ReportIssue);
        //    Add("Cashier", PermissionCode.AccessHotlineSupport);

        //    // InventoryStaff
        //    Add("InventoryStaff", PermissionCode.ManageProduct);
        //    Add("InventoryStaff", PermissionCode.AddProduct);
        //    Add("InventoryStaff", PermissionCode.EditProduct);
        //    Add("InventoryStaff", PermissionCode.DeleteProduct);
        //    Add("InventoryStaff", PermissionCode.ImportInventory);
        //    Add("InventoryStaff", PermissionCode.ViewOrder);
        //    Add("InventoryStaff", PermissionCode.ViewDashboard);

        //    // Accountant
        //    Add("Accountant", PermissionCode.ManageInvoice);
        //    Add("Accountant", PermissionCode.DraftInvoice);
        //    Add("Accountant", PermissionCode.SignInvoice);
        //    Add("Accountant", PermissionCode.ExportInvoice);
        //    Add("Accountant", PermissionCode.ViewTransactionHistory);
        //    Add("Accountant", PermissionCode.SearchTransaction);
        //    Add("Accountant", PermissionCode.ViewReport);

        //    // CustomerSupport
        //    Add("CustomerSupport", PermissionCode.FeedbackSubmit);
        //    Add("CustomerSupport", PermissionCode.ReportIssue);
        //    Add("CustomerSupport", PermissionCode.AccessHotlineSupport);
        //    Add("CustomerSupport", PermissionCode.ViewTransactionHistory);
        //    Add("CustomerSupport", PermissionCode.SearchTransaction);

        //    // Admin
        //    Add("Admin", PermissionCode.ApproveMerchant);
        //    Add("Admin", PermissionCode.ConfigureSecurityDevice);
        //    Add("Admin", PermissionCode.RegisterMerchant);
        //    Add("Admin", PermissionCode.ManageMerchant);
        //    Add("Admin", PermissionCode.DeclareDevice);
        //    Add("Admin", PermissionCode.CheckDeviceStatus);
        //    Add("Admin", PermissionCode.ManageUsers);
        //    Add("Admin", PermissionCode.CreateUser);
        //    Add("Admin", PermissionCode.AssignRole);
        //    Add("Admin", PermissionCode.ViewAuditLog);
        //    return list;
        //}
    }
}
