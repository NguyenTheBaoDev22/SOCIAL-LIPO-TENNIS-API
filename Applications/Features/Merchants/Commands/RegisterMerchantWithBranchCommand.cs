using Applications.Features.Merchants.DTOs;
using Core.Enums;
using MediatR;
using Shared.Results;
using System.ComponentModel.DataAnnotations;

public class RegisterMerchantWithBranchCommand : IRequest<BaseResponse<RegisterMerchantWithBranchRes>>
{
    // Thông tin về Merchant
    [Required(ErrorMessage = "Merchant Name is required.")]
    [MaxLength(255, ErrorMessage = "Merchant Name cannot exceed 255 characters.")]
    public string MerchantName { get; set; }

    [Required(ErrorMessage = "Business Registration Number is required.")]
    [MaxLength(255, ErrorMessage = "Business Registration Number cannot exceed 255 characters.")]
    public string BusinessRegistrationNo { get; set; }
    [Required]
    public List<string> BusinessRegistrationImageUrls { get; set; }
    [Required(ErrorMessage = "Business Address is required.")]
    [MaxLength(255, ErrorMessage = "Business Address cannot exceed 255 characters.")]
    public string BusinessAddress { get; set; }

    //[Required]
    //public string PrimaryTaxNumber { get; set; }
    // [Required(ErrorMessage = "OwnerIdCardNumber is required.")]
    public string OwnerIdCardNumber { get; set; } = string.Empty;
    [Required(ErrorMessage = "Owner Name is required.")]
    [MaxLength(255, ErrorMessage = "Owner Name cannot exceed 255 characters.")]
    public string OwnerName { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid Email format.")]
    [MaxLength(255, ErrorMessage = "Email cannot exceed 255 characters.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Phone is required.")]
    [MaxLength(10, ErrorMessage = "Phone number cannot exceed 10 characters.")]
    public string Phone { get; set; }

    // Thông tin về Merchant
    // [Required(ErrorMessage = "Merchant Type is required.")]
    [MaxLength(50, ErrorMessage = "Merchant Type cannot exceed 50 characters.")]
    [RegularExpression(@"^(Household|Company)$", ErrorMessage = "Merchant Type must be 'Household' or 'Company'.")]
    public string MerchantType { get; set; } = "Household";

    //[Required(ErrorMessage = "Tax Number is required.")]
    //[MaxLength(13, ErrorMessage = "Tax Number cannot exceed 13 characters.")]
    //[RegularExpression(@"^[0-9]+$", ErrorMessage = "Tax Number must contain only numeric digits.")]
    //public string TaxNumber { get; set; }

    //// Thông tin về chi nhánh
    //[Required(ErrorMessage = "Branch Name is required.")]
    //[MaxLength(255, ErrorMessage = "Branch Name cannot exceed 255 characters.")]
    //public string BranchName { get; set; }

    //[Required(ErrorMessage = "Branch Address is required.")]
    //[MaxLength(80, ErrorMessage = "Branch Address cannot exceed 80 characters.")]
    //public string BranchAddress { get; set; }

    // [Required(ErrorMessage = "Province Name is required.")]
    [MaxLength(35, ErrorMessage = "Province Name cannot exceed 35 characters.")]
    public string ProvinceName { get; set; } = string.Empty;
    // [Required]
    public string ProvinceCode { get; set; } = string.Empty;

    // [Required(ErrorMessage = "Commune Name is required.")]
    [MaxLength(35, ErrorMessage = "Commune Name cannot exceed 35 characters.")]
    public string CommuneName { get; set; } = string.Empty;
    // [Required]
    public string CommuneCode { get; set; } = string.Empty;
    // [Required]
    public string StreetAddress { get; set; } = string.Empty;
   // [Required(ErrorMessage = "Latitude is required.")]
    // public double? Latitude { get; set; }
    // public string? Latitude { get; set; }

   // [Required(ErrorMessage = "Longitude is required.")]
    // public double? Longitude { get; set; }


    //[EmailAddress(ErrorMessage = "Invalid Branch Email format.")]
    //[MaxLength(80, ErrorMessage = "Branch Email cannot exceed 80 characters.")]
    //public string? BranchEmail { get; set; }

    //[MaxLength(10, ErrorMessage = "Branch Phone cannot exceed 10 characters.")]
    //public string? BranchPhone { get; set; }

    // [Required(ErrorMessage = "Exterior Images are required.")]
    public List<string>? ExteriorImagesUrl { get; set; }

    // [Required(ErrorMessage = "Interior Images are required.")]
    public List<string>? InteriorImagesUrl { get; set; }

    // Thông tin tài chính
    [Required(ErrorMessage = "Bank Account Number is required.")]
    [MaxLength(20, ErrorMessage = "Bank Account Number cannot exceed 20 characters.")]
    public string BankAccountNumber { get; set; }

    [Required(ErrorMessage = "Bank Name is required.")]
    [MaxLength(255, ErrorMessage = "Bank Name cannot exceed 255 characters.")]
    public string BankName { get; set; }

    [Required(ErrorMessage = "Bank Account Holder is required.")]
    [MaxLength(255, ErrorMessage = "Bank Account Holder cannot exceed 255 characters.")]
    public string BankAccountHolder { get; set; }

    // Thông tin tài liệu
    [Required(ErrorMessage = "ID Card Front image is required.")]
    public string OwnerIdCardFrontUrl { get; set; }

    [Required(ErrorMessage = "ID Card Back image is required.")]
    public string OwnerIdCardBackUrl { get; set; }

    //  [Required(ErrorMessage = "Signed Form 01A is required.")]
    public string? SignedForm01AUrl { get; set; }
    // [Required(ErrorMessage = "MerchantCategoryCode is required.")]
    public string MerchantCategoryCode { get; set; } = MerchantCategoryCodes.DepartmentStore;
    // Todo: Thêm thông tin kế toán và 2 mặt CCCD của kế toán, giấy phép phụ nếu có
    public string? ActiveCallbackUrl { get; set; }
}
