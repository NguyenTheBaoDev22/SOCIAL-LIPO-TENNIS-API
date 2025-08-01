namespace Shared.Results
{
    /// <summary>
    /// Business-level error codes (chuẩn hóa dạng 4 số: 01XX)
    /// </summary>
    /// <summary>
    /// Business-level error codes (chuẩn hóa dạng 4 số: 01XX)
    /// </summary>
    public static class ErrorCodes
    {
        // === 01XX: Merchant related errors ===
        public const string Merchant_DuplicateBusinessNo = "0101";       // Trùng mã số kinh doanh
        public const string Merchant_DuplicateTaxNo = "0102";            // Trùng mã số thuế
        public const string Merchant_DuplicateBankAccount = "0103";      // Trùng số tài khoản
        public const string Merchant_InvalidCategoryCode = "0104";       // MCC không hợp lệ
        public const string Merchant_InvalidProvinceCode = "0105";       // Mã tỉnh không tồn tại
        public const string Merchant_InvalidCommuneCode = "0106";        // Mã xã không tồn tại
        public const string Merchant_NotFound = "0107";                   // Mã thương nhân không tồn tại
        public const string Merchant_AccountLocked = "0108";              // Thương nhân đã bị khóa
        public const string Merchant_Unauthenticated = "0109";            // Thương nhân chưa xác thực
        public const string Merchant_CreationFailed = "0110";             // Lỗi khi tạo merchant
        public const string Merchant_InvalidCode = "0111";                //Không đúng mã merchant
        public const string MerchantBranch_DuplicateBranchTaxNo = "0112";//Trùng mã số thuế địa điểm kinh doanh
        public const string Merchant_InvalidBranch = "0113";// Không tồn tại merchant branch
        public const string Terminal_ExceedLimit = "0114";// Số lượng máy bán hàng của merchant đã đạt giới hạn 99
        public const string MerchantBranch_CodeMismatch = "0115";//Không trùng khớp mã merchant với Merchantid
        public const string Merchant_CodeMismatch = "0116";//Không trùng khớp mã merchant branch với MerchantBranchId  
        public const string Terminal_DuplicateSerialNumber = "0117";//Đã tồn tại serial đang hoạt động trong PaymentTerminal
        public const string MerchantBranch_Inactive = "0118";// Địa điểm kinh doanh chưa được kích hoạt
        public const string Merchant_Inactive = "0119";//Merchant chưa được kích hoạt
        public const string MerchantBranch_NotFound = "0120"; // Không tìm thấy merchant branch
        public const string MerchantBranch_AlreadyApproved = "0121";
        public const string PaymentTerminal_NotFound = "0122";
        public const string MerchantBranchTaxNo_NotFound = "0123"; // Không tìm thấy merchant branch

        // === 02XX: Payment related errors ===
        public const string Payment_Failed = "0201";                      // Thanh toán lỗi
        public const string Payment_InsufficientFunds = "0202";           // Số dư không đủ để thanh toán
        public const string Payment_CardExpired = "0203";                 // Thẻ tín dụng hết hạn
        public const string Payment_CardBlocked = "0204";                 // Thẻ bị khóa
        public const string Payment_FraudSuspected = "0205";              // Thanh toán bị từ chối do nghi ngờ gian lận
        public const string Payment_Suspended = "0206";                   // Thanh toán bị tạm ngưng
        public const string Payment_ConnectionError = "0207";             // Không thể kết nối đến hệ thống thanh toán
        public const string Payment_GenerateQrFailed = "0308";

        // === 03XX: Authentication/Account related errors ===
        public const string Auth_InvalidOTP = "0301";                     // Sai OTP
        public const string Auth_AccountNotFound = "0302";                // Tài khoản không tồn tại
        public const string Auth_AccountLocked = "0303";                  // Tài khoản bị khóa
        public const string Auth_AccountDeleted = "0304";                 // Tài khoản đã bị xóa
        public const string Auth_InsufficientPermissions = "0305";        // Tài khoản không có quyền truy cập
        public const string Auth_InvalidPassword = "0306";                // Mật khẩu không hợp lệ
        public const string Auth_AuthenticationFailed = "0307";           // Xác thực không thành công
        public const string Auth_AccountAlreadyExists = "0308";           // Tài khoản đã tồn tại
        public const string User_AlreadyExists = "0309";
        public const string Merchant_MissingPrimaryEmail = "0310";
        public const string User_InvalidPassword = "0311";
        public const string User_NotFound = "0312";
        // === 04XX: Order related errors ===
        public const string Order_CreationFailed = "0401";                // Lỗi khi tạo đơn hàng
        public const string Order_AlreadyExists = "0402";                 // Đơn hàng đã tồn tại
        public const string Order_Cancelled = "0403";                     // Đơn hàng đã bị hủy
        public const string Order_NotFound = "0404";                      // Đơn hàng không tìm thấy
        public const string Order_FulfillmentFailed = "0405";             // Đơn hàng không thể hoàn thành
        public const string Order_DataError = "0406";                     // Đơn hàng bị lỗi dữ liệu
        public const string Order_Declined = "0407";                      // Đơn hàng bị từ chối

        // === 99XX: System related errors ===
        public const string System_Exception = "9901";                    // Exception hệ thống
        public const string System_DBError = "9902";                      // Lỗi kết nối cơ sở dữ liệu
        public const string System_APIError = "9903";                     // Lỗi API không xác định
        public const string System_ServerError = "9904";                  // Lỗi máy chủ
        public const string System_IOError = "9905";                      // Lỗi khi đọc/ghi dữ liệu
        public const string System_ExternalServiceError = "9906";         // Lỗi khi gọi đến dịch vụ bên ngoài
        public const string System_TemporaryError = "9907";               // Lỗi hệ thống tạm thời

        // === 05XX: Resource Management related errors ===
        public const string Resource_Insufficient = "0501";               // Tài nguyên không đủ (bộ nhớ, CPU)
        public const string Resource_FileUploadError = "0502";            // Lỗi khi tải tệp tin
        public const string Resource_DataSaveError = "0503";              // Lỗi khi lưu trữ dữ liệu
        public const string Resource_NotFound = "0504";                   // Không tìm thấy tài nguyên
        public const string Resource_Locked = "0505";                     // Tài nguyên bị khóa
        public const string ShopInventory_InvalidProduct = "0506";        // Sản phẩm không hợp lệ
        public const string ShopInventory_StockQuantityExceeded = "0507"; // Số lượng hàng tồn kho vượt mức
        public const string ShopInventory_AuditFailed = "0508";           // Kiểm kê kho không thành công
        public const string ProductCategory_AlreadyExists = "0509";       // Danh mục sản phẩm đã tồn tại
        public const string ProductCategory_NotFound = "06510";            // Không tìm thấy danh mục sản phẩm
        // === 06XX: Authorization and Permissions related errors ===
        public const string Authorization_AccessDenied = "0601";          // Quyền truy cập bị từ chối
        public const string Authorization_InsufficientRights = "0602";    // Người dùng không có quyền thực hiện hành động này
        public const string Authorization_InvalidPermissions = "0603";    // Quyền truy cập không hợp lệ
        public const string Authorization_AdminRequired = "0604";         // Cần quyền quản trị để thực hiện hành động
        public const string Role_NotFound = "0605";

        // === 07XX: API/Service related errors ===
        public const string API_Unavailable = "0701";                     // API không khả dụng
        public const string API_ErrorResponse = "0702";                   // API trả về lỗi
        public const string API_ServiceUnavailable = "0703";              // Dịch vụ không thể truy cập
        public const string API_AuthenticationFailed = "0704";            // Lỗi xác thực API
        public const string API_ExternalServiceError = "0705";            // Lỗi khi gọi service thứ ba

        // === 08XX: System Notifications related errors ===
        public const string Notification_UpdateFailed = "0801";           // Cập nhật không thành công
        public const string Notification_Invalid = "0802";                // Thông báo không hợp lệ
        public const string Notification_NotFound = "0803";               // Thông báo không tồn tại
        public const string Notification_Unprocessed = "0804";            // Thông báo chưa được xử lý

        // === 09XX: Session/Token/Identity related errors ===
        public const string Session_Expired = "0901";                     // Phiên làm việc đã hết hạn
        public const string Session_IdentityError = "0902";               // Lỗi định danh
        public const string Session_TokenError = "0903";                  // Lỗi xác thực token
    }
}
//01XX Nhóm Merchant	0101 = Trùng mã số
//02XX	Nhóm Payment	0201 = Thanh toán lỗi
//03XX	Nhóm Auth/Account	0301 = Sai OTP
//04XX	Nhóm Order	0401 = Đơn hàng lỗi
//99XX	Lỗi hệ thống, unknown	9901 = Exception