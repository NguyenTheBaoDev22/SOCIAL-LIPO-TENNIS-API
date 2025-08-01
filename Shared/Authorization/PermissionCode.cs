using System.ComponentModel;

namespace Shared.Authorization
{
    public enum PermissionCode
    {
        [Description("Xem tổng quan Dashboard")]
        ViewDashboard,

        [Description("Đăng ký Merchant mới")]
        RegisterMerchant,

        [Description("Duyệt hồ sơ Merchant")]
        ApproveMerchant,

        [Description("Cập nhật thông tin Merchant")]
        UpdateMerchantInfo,

        [Description("Thêm chi nhánh mới")]
        AddBranch,

        [Description("Cập nhật chi nhánh")]
        UpdateBranch,

        [Description("Khai báo thiết bị")]
        DeclareDevice,

        [Description("Kiểm tra trạng thái thiết bị")]
        CheckDeviceStatus,

        [Description("Tạo đơn hàng")]
        CreateOrder,

        [Description("Chỉnh sửa đơn hàng")]
        EditOrder,

        [Description("Hủy đơn hàng")]
        CancelOrder,

        [Description("Xem danh sách đơn hàng")]
        ViewOrder,

        [Description("Quản lý hóa đơn")]
        ManageInvoice,

        [Description("Lưu nháp hóa đơn")]
        DraftInvoice,

        [Description("Ký hóa đơn")]
        SignInvoice,

        [Description("Xuất hóa đơn")]
        ExportInvoice,

        [Description("Quản lý sản phẩm")]
        ManageProduct,

        [Description("Thêm sản phẩm")]
        AddProduct,

        [Description("Chỉnh sửa sản phẩm")]
        EditProduct,

        [Description("Xóa sản phẩm")]
        DeleteProduct,

        [Description("Nhập tồn kho")]
        ImportInventory,

        [Description("Quản lý người dùng")]
        ManageUsers,

        [Description("Tạo người dùng")]
        CreateUser,

        [Description("Phân quyền vai trò")]
        AssignRole,

        [Description("Xem lịch sử hoạt động")]
        ViewUserActivity,

        [Description("Xem báo cáo")]
        ViewReport,

        [Description("Xuất báo cáo")]
        ExportReport,

        [Description("Xem lịch sử giao dịch")]
        ViewTransactionHistory,

        [Description("Tìm kiếm giao dịch")]
        SearchTransaction,

        [Description("Gửi yêu cầu mua hàng")]
        RequestPurchase,

        [Description("Gửi phản hồi")]
        FeedbackSubmit,

        [Description("Báo cáo sự cố")]
        ReportIssue,

        [Description("Truy cập hỗ trợ đường dây nóng")]
        AccessHotlineSupport,

        [Description("Cấu hình thiết bị bảo mật (HSM, USB Token, SIM PKI)")]
        ConfigureSecurityDevice,

        [Description("Quản lý thiết bị")]
        ManageDevice,

        [Description("Quản lý đơn hàng")]
        ManageOrder,

        [Description("Quản lý Merchant")]
        ManageMerchant,
        [Description("Xem lịch sử biến động data")]
        ViewAuditLog

    }
}
