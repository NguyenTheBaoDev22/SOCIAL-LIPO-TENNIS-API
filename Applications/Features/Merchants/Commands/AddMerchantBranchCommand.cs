using Applications.Features.Merchants.DTOs;
using MediatR;
using Shared.Results;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Command để thêm địa điểm bán hàng mới cho merchant đã tồn tại.
/// </summary>
public class AddMerchantBranchCommand : IRequest<BaseResponse<AddMerchantBranchRes>>
{
    /// <summary>
    /// Id của merchant đã tồn tại
    /// </summary>
    [Required(ErrorMessage = "MerchantId is required.")]
    public Guid MerchantId { get; set; }

    /// <summary>
    /// Mã merchant, cần khớp với MerchantId để xác thực
    /// </summary>
    [Required(ErrorMessage = "MerchantCode is required.")]
    public string MerchantCode { get; set; }

    /// <summary>
    /// Tên chi nhánh mới
    /// </summary>
    [Required(ErrorMessage = "BranchName is required.")]
    public string BranchName { get; set; }

    /// <summary>
    /// Địa chỉ chi nhánh mới
    /// </summary>
    [Required(ErrorMessage = "BranchAddress is required.")]
    public string BranchAddress { get; set; }

    [Required(ErrorMessage = "ProvinceCode is required.")]
    public string ProvinceCode { get; set; }

    [Required(ErrorMessage = "ProvinceName is required.")]
    public string ProvinceName { get; set; }

    [Required(ErrorMessage = "CommuneCode is required.")]
    public string CommuneCode { get; set; }

    [Required(ErrorMessage = "CommuneName is required.")]
    public string CommuneName { get; set; }

    /// <summary>
    /// Email của chi nhánh (có thể null)
    /// </summary>
    public string? BranchEmail { get; set; }

    /// <summary>
    /// Số điện thoại của chi nhánh (có thể null)
    /// </summary>
    public string? BranchPhone { get; set; }

    /// <summary>
    /// Danh sách ảnh bên ngoài chi nhánh
    /// </summary>
    public List<string>? ExteriorImages { get; set; }

    /// <summary>
    /// Danh sách ảnh bên trong chi nhánh
    /// </summary>
    public List<string>? InteriorImages { get; set; }

    /// <summary>
    /// Tọa độ vị trí
    /// </summary>
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    /// <summary>
    /// Số tài khoản ngân hàng của chi nhánh
    /// </summary>
    [Required(ErrorMessage = "BankAccountNumber is required.")]
    public string BankAccountNumber { get; set; }

    /// <summary>
    /// Tên ngân hàng
    /// </summary>
    [Required(ErrorMessage = "BankName is required.")]
    public string BankName { get; set; }

    /// <summary>
    /// Chủ tài khoản ngân hàng
    /// </summary>
    [Required(ErrorMessage = "BankAccountHolder is required.")]
    public string BankAccountHolder { get; set; }

    /// <summary>
    /// Link file mẫu đơn đã ký (01A)
    /// </summary>
    public string? SignedForm01AUrl { get; set; }

    /// <summary>
    /// Mã ngành nghề MCC
    /// </summary>
    [Required(ErrorMessage = "MerchantCategoryCode is required.")]
    public string MerchantCategoryCode { get; set; }

    [Required(ErrorMessage = "BranchTaxNumber is required.")]
    /// <summary>
    /// Mã số thuế chi nhánh (nếu có)
    /// </summary>
    public string? BranchTaxNumber { get; set; }

    public string? ActiveCallbackUrl { get; set; }
}
