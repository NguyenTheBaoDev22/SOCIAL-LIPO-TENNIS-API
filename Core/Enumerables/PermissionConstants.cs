using Shared.Authorization;

namespace Core.Enumerables
{

    public static class PermissionConstants
    {
        public static List<string> All = Enum.GetNames(typeof(PermissionCode)).ToList();

        public static readonly Dictionary<PermissionCode, Guid> Map = new()
    {
        { PermissionCode.ViewDashboard, Guid.Parse("00000000-0000-0000-0000-000000000001") },
        { PermissionCode.RegisterMerchant, Guid.Parse("00000000-0000-0000-0000-000000000002") },
        { PermissionCode.ApproveMerchant, Guid.Parse("00000000-0000-0000-0000-000000000003") },
        { PermissionCode.UpdateMerchantInfo, Guid.Parse("00000000-0000-0000-0000-000000000004") },
        { PermissionCode.AddBranch, Guid.Parse("00000000-0000-0000-0000-000000000005") },
        { PermissionCode.UpdateBranch, Guid.Parse("00000000-0000-0000-0000-000000000006") },
        { PermissionCode.DeclareDevice, Guid.Parse("00000000-0000-0000-0000-000000000007") },
        { PermissionCode.CheckDeviceStatus, Guid.Parse("00000000-0000-0000-0000-000000000008") },
        { PermissionCode.CreateOrder, Guid.Parse("00000000-0000-0000-0000-000000000009") },
        { PermissionCode.EditOrder, Guid.Parse("00000000-0000-0000-0000-00000000000A") },
        { PermissionCode.CancelOrder, Guid.Parse("00000000-0000-0000-0000-00000000000B") },
        { PermissionCode.ViewOrder, Guid.Parse("00000000-0000-0000-0000-00000000000C") },
        { PermissionCode.ManageInvoice, Guid.Parse("00000000-0000-0000-0000-00000000000D") },
        { PermissionCode.DraftInvoice, Guid.Parse("00000000-0000-0000-0000-00000000000E") },
        { PermissionCode.SignInvoice, Guid.Parse("00000000-0000-0000-0000-00000000000F") },
        { PermissionCode.ExportInvoice, Guid.Parse("00000000-0000-0000-0000-000000000010") },
        { PermissionCode.ManageProduct, Guid.Parse("00000000-0000-0000-0000-000000000011") },
        { PermissionCode.AddProduct, Guid.Parse("00000000-0000-0000-0000-000000000012") },
        { PermissionCode.EditProduct, Guid.Parse("00000000-0000-0000-0000-000000000013") },
        { PermissionCode.DeleteProduct, Guid.Parse("00000000-0000-0000-0000-000000000014") },
        { PermissionCode.ImportInventory, Guid.Parse("00000000-0000-0000-0000-000000000015") },
        { PermissionCode.ManageUsers, Guid.Parse("00000000-0000-0000-0000-000000000016") },
        { PermissionCode.CreateUser, Guid.Parse("00000000-0000-0000-0000-000000000017") },
        { PermissionCode.AssignRole, Guid.Parse("00000000-0000-0000-0000-000000000018") },
        { PermissionCode.ViewUserActivity, Guid.Parse("00000000-0000-0000-0000-000000000019") },
        { PermissionCode.ViewReport, Guid.Parse("00000000-0000-0000-0000-00000000001A") },
        { PermissionCode.ExportReport, Guid.Parse("00000000-0000-0000-0000-00000000001B") },
        { PermissionCode.ViewTransactionHistory, Guid.Parse("00000000-0000-0000-0000-00000000001C") },
        { PermissionCode.SearchTransaction, Guid.Parse("00000000-0000-0000-0000-00000000001D") },
        { PermissionCode.RequestPurchase, Guid.Parse("00000000-0000-0000-0000-00000000001E") },
        { PermissionCode.FeedbackSubmit, Guid.Parse("00000000-0000-0000-0000-00000000001F") },
        { PermissionCode.ReportIssue, Guid.Parse("00000000-0000-0000-0000-000000000020") },
        { PermissionCode.AccessHotlineSupport, Guid.Parse("00000000-0000-0000-0000-000000000021") },
        { PermissionCode.ConfigureSecurityDevice, Guid.Parse("00000000-0000-0000-0000-000000000022") },
        { PermissionCode.ManageDevice, Guid.Parse("00000000-0000-0000-0000-000000000023") },
        { PermissionCode.ManageOrder, Guid.Parse("00000000-0000-0000-0000-000000000024") },
        { PermissionCode.ManageMerchant, Guid.Parse("00000000-0000-0000-0000-000000000025") },
    };
    }
}
