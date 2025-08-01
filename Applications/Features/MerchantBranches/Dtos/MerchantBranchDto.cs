using System.ComponentModel.DataAnnotations;

namespace Applications.Features.MerchantBranches.Dtos
{
    public class MerchantBranchDto
    {
        public Guid Id { get; set; }
        [Required]
        [MaxLength(3)]
        public string MerchantBranchCode { get; set; }  // Mã điểm bán hàng (3 ký tự)
        public string BranchName { get; set; }  // Tên điểm bán hàng
        public string BranchAddress { get; set; }  // Địa chỉ của điểm bán

        // Mối quan hệ 1 - N với PaymentTerminal
        // public ICollection<PaymentTerminal> Terminals { get; set; }  
        public string MerchantCode { get; set; }
        // Mối quan hệ với Merchant
        public Guid MerchantId { get; set; }
        // public virtual Merchant Merchant { get; set; }  
        public string? BranchEmail { get; set; } // Email của riêng chi nhánh
        public int VerificationAttempts { get; set; } = 0;  // Số lần thẩm định của Napas (mặc định là 0)
        public string VerificationStatus { get; set; }  //VerificationStatusConstants Trạng thái thẩm định của merchant
        public List<string>? ExteriorImages { get; set; }  // Danh sách URL hình ảnh bên ngoài cửa hàng, bao gồm bảng hiệu thể hiện địa chỉ
        public List<string>? InteriorImages { get; set; }  // Danh sách URL hình ảnh bên trong cửa hàng, sản phẩm trưng bày, quầy tính tiền, v.v.
        // Các thông tin về vị trí định vị trên bản đồ
        public string ProvinceCode { get; set; }
        public string ProvinceName { get; set; }
        public string CommuneCode { get; set; }
        public string CommuneName { get; set; }
        public double? Latitude { get; set; }  // Vị trí vĩ độ của cửa hàng trên bản đồ (tọa độ GPS)

        public double? Longitude { get; set; }  // Vị trí kinh độ của cửa hàng trên bản đồ (tọa độ GPS)
        public string Status { get; set; }
    }

    public class CreateMerchantBranchReq
    {
        [Required(ErrorMessage = "Bắt buộc cung cấp mã Merchant")]
        public string MerchantCode { get; set; }

        [Required(ErrorMessage = "Bắt buộc cung cấp MerchantId")]
        public Guid MerchantId { get; set; }

        [Required(ErrorMessage = "Bắt buộc cung cấp mã điểm bán hàng")]
        [MaxLength(3, ErrorMessage = "Mã điểm bán hàng không vượt quá 3 ký tự")]
        public string MerchantBranchCode { get; set; }  // Mã điểm bán hàng (3 ký tự)

        // Mã số thuế của địa điểm kinh doanh
        [Required(ErrorMessage = "Bắt buộc cung cấp mã số thuế")]
        [MaxLength(13, ErrorMessage = "Mã số thuế không vượt quá 13 ký tự")]
        [RegularExpression(@"^[A-Za-z0-9]+$", ErrorMessage = "Mã số thuế không được chứa ký tự đặc biệt")]
        public string TaxNumber { get; set; }  // Mã số thuế của địa điểm kinh doanh

        [Required(ErrorMessage = "Bắt buộc cung cấp tên điểm bán hàng")]
        public string BranchName { get; set; }  // Tên điểm bán hàng

        [Required(ErrorMessage = "Bắt buộc cung cấp địa chỉ của điểm bán")]
        public string BranchAddress { get; set; }  // Địa chỉ của điểm bán

        [Required(ErrorMessage = "Bắt buộc cung cấp mã tỉnh")]
        public string ProvinceCode { get; set; }

        [Required(ErrorMessage = "Bắt buộc cung cấp tên tỉnh")]
        public string ProvinceName { get; set; }

        [Required(ErrorMessage = "Bắt buộc cung cấp mã xã")]
        public string CommuneCode { get; set; }

        [Required(ErrorMessage = "Bắt buộc cung cấp tên xã")]
        public string CommuneName { get; set; }

        public double? Latitude { get; set; }  // Vị trí vĩ độ của cửa hàng trên bản đồ (tọa độ GPS)

        public double? Longitude { get; set; }  // Vị trí kinh độ của cửa hàng trên bản đồ (tọa độ GPS)

        public string? BranchEmail { get; set; } // Email của riêng chi nhánh
        public string? BranchPhone { get; set; } // Phone của riêng chi nhánh

        [Required(ErrorMessage = "Bắt buộc cung cấp hình ảnh bên ngoài cửa hàng")]
        public List<string> ExteriorImages { get; set; }  // Danh sách URL hình ảnh bên ngoài cửa hàng, bao gồm bảng hiệu thể hiện địa chỉ

        [Required(ErrorMessage = "Bắt buộc cung cấp hình ảnh bên trong cửa hàng")]
        public List<string> InteriorImages { get; set; }  // Danh sách URL hình ảnh bên trong cửa hàng, sản phẩm trưng bày, quầy tính tiền, v.v.
    }
    public class CreateMerchantBranchRes
    {
        public Guid Id { get; set; }
        [Required]
        [MaxLength(3)]
        public string MerchantBranchCode { get; set; }  // Mã điểm bán hàng (3 ký tự)
        public string BranchName { get; set; }  // Tên điểm bán hàng
        public string BranchAddress { get; set; }  // Địa chỉ của điểm bán
        public string MerchantCode { get; set; }
        // Mối quan hệ với Merchant
        public Guid MerchantId { get; set; }
        // public virtual Merchant Merchant { get; set; }
        public string? BranchEmail { get; set; } // Email của riêng chi nhánh
        public int VerificationAttempts { get; set; } = 0;  // Số lần thẩm định của Napas (mặc định là 0)
        public string VerificationStatus { get; set; }  //VerificationStatusConstants Trạng thái thẩm định của merchant
        public List<string>? ExteriorImages { get; set; }  // Danh sách URL hình ảnh bên ngoài cửa hàng, bao gồm bảng hiệu thể hiện địa chỉ
        public List<string>? InteriorImages { get; set; }  // Danh sách URL hình ảnh bên trong cửa hàng, sản phẩm trưng bày, quầy tính tiền, v.v.
        // Các thông tin về vị trí định vị trên bản đồ
        public string ProvinceCode { get; set; }
        public string ProvinceName { get; set; }
        public string CommuneCode { get; set; }
        public string CommuneName { get; set; }
        public double? Latitude { get; set; }  // Vị trí vĩ độ của cửa hàng trên bản đồ (tọa độ GPS)

        public double? Longitude { get; set; }  // Vị trí kinh độ của cửa hàng trên bản đồ (tọa độ GPS)
        public string Status { get; set; }
    }


    public class MerchantBranchRes
    {
        public Guid MerchantId { get; set; }
        public string MerchantName { get; set; }
        public string MerchantType { get; set; }  // Loại merchant: "Hộ kinh doanh" hoặc "Doanh nghiệp công ty"
        public string MerchantCode { get; set; }
        public bool IsMerchantActive { get; set; } = false;

        public Guid MerchantBranchId { get; set; }
        public string MerchantBranchCode { get; set; }  // Mã điểm bán hàng (3 ký tự)
        public string BranchName { get; set; }  // Tên điểm bán hàng
        public string BranchAddress { get; set; }  // Địa chỉ của điểm bán

        public string? BranchEmail { get; set; } // Email của riêng chi nhánh
        public int VerificationAttempts { get; set; } = 0;  // Số lần thẩm định của Napas (mặc định là 0)
        public string VerificationStatus { get; set; }  //VerificationStatusConstants Trạng thái thẩm định của merchant
        public List<string>? ExteriorImages { get; set; }  // Danh sách URL hình ảnh bên ngoài cửa hàng, bao gồm bảng hiệu thể hiện địa chỉ
        public List<string>? InteriorImages { get; set; }  // Danh sách URL hình ảnh bên trong cửa hàng, sản phẩm trưng bày, quầy tính tiền, v.v.
        // Các thông tin về vị trí định vị trên bản đồ
        public string ProvinceCode { get; set; }
        public string ProvinceName { get; set; }
        public string CommuneCode { get; set; }
        public string CommuneName { get; set; }
        public double? Latitude { get; set; }  // Vị trí vĩ độ của cửa hàng trên bản đồ (tọa độ GPS)

        public double? Longitude { get; set; }  // Vị trí kinh độ của cửa hàng trên bản đồ (tọa độ GPS)
        public string Status { get; set; }
    }
    public class MerchantBranchStatusRes
    {
        public Guid MerchantId { get; set; }
        public string MerchantCode { get; set; }
        public string MerchantName { get; set; }
        public string MerchantType { get; set; }  // Loại merchant: "Hộ kinh doanh" hoặc "Doanh nghiệp công ty"
        public bool IsMerchantActive { get; set; }
        public Guid MerchantBranchId { get; set; }
        public string MerchantBranchCode { get; set; }
        public string BranchName { get; set; }  // Tên điểm bán hàng
        public int VerificationAttempts { get; set; } = 0;  // Số lần thẩm định của Napas (mặc định là 0)
        public string VerificationStatus { get; set; }  //VerificationStatusConstants Trạng thái thẩm định của merchant
        public string Status { get; set; }
    }


    /// <summary>
    /// DTO trả về thông tin Merchant và MerchantBranch theo mã số thuế.
    /// </summary>
    public class GetMerchantBranchByTaxNumberRes
    {
        /// <summary>
        /// Id của merchant.
        /// </summary>
        public Guid MerchantId { get; set; }

        /// <summary>
        /// Mã merchant.
        /// </summary>
        public string MerchantCode { get; set; }

        /// <summary>
        /// Id của chi nhánh (branch).
        /// </summary>
        public Guid MerchantBranchId { get; set; }

        /// <summary>
        /// Mã chi nhánh (3 ký tự).
        /// </summary>
        public string MerchantBranchCode { get; set; }

        /// <summary>
        /// Tên chi nhánh.
        /// </summary>
        public string BranchName { get; set; }

        /// <summary>
        /// Email của chi nhánh (có thể null).
        /// </summary>
        public string? BranchEmail { get; set; }
    }

}
