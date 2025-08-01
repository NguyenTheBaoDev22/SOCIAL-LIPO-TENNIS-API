using Applications.Features.MerchantBranches.Dtos;
using MediatR;
using Shared.Results;
using System.ComponentModel.DataAnnotations;

namespace Applications.Features.MerchantBranches.Commands
{
    public class CreateMerchantBranchCommand : IRequest<BaseResponse<CreateMerchantBranchRes>>
    {
        [Required(ErrorMessage = "Bắt buộc cung cấp mã Merchant")]
        public string MerchantCode { get; set; }

        [Required(ErrorMessage = "Bắt buộc cung cấp MerchantId")]
        public Guid MerchantId { get; set; }
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
}
