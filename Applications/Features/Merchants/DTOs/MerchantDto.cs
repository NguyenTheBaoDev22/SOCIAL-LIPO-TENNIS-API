namespace Applications.Features.Merchants.DTOs
{
    /// <summary>
    /// Merchant Data Transfer Object
    /// </summary>
    public class MerchantDto
    {
        // ID của Merchant
        public Guid Id { get; set; }

        // Mã Merchant, có thể là mã tự sinh
        public string MerchantCode { get; set; }

        // Tên của Merchant
        public string MerchantName { get; set; }

        // Địa chỉ email của Merchant
        public string Email { get; set; }
        public string BusinessRegistrationNo { get; set; }
    }


    public class RegisterMerchantWithBranchRes
    {
        // ID của Merchant
        public Guid MerchantId { get; set; }
        public string MerchantCode { get; set; }
        public Guid MerchantBranchId { get; set; }
        public string MerchantBranchCode { get; set; }
        public bool IsMerchantBranchActive { get; set; }
    }
    public class AddMerchantBranchRes
    {
        public Guid MerchantId { get; set; }
        public string MerchantCode { get; set; }
        public Guid MerchantBranchId { get; set; }
        public string MerchantBranchCode { get; set; }
        public bool IsMerchantBranchActive { get; set; }
    }

    public class AddPaymentTerminalRes
    {
        public Guid TerminalId { get; set; }
        public string TerminalCode { get; set; }
        public string CombinedIdentifier { get; set; }
    }
    public class BusinessTypeResponse
    {
        public string BusinessTypeCode { get; set; }
        public string BusinessTypeName { get; set; }
    }
}
