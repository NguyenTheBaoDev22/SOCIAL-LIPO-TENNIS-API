namespace Core.Enums
{
    /// <summary>
    /// Mã ngành nghề merchant theo tiêu chuẩn ISO 18245:2023
    /// </summary>
    public static class MerchantCategoryCodes
    {
        // Constants để sử dụng dễ dàng, rõ ràng
        public const string Unknown = "0000";
        public const string GroceryStores = "5411";
        public const string Restaurant = "5812";
        public const string FastFood = "5814";
        public const string Pharmacy = "5912";
        public const string MiscRetail = "5999";
        public const string FinancialServices = "6011";
        public const string LocalTransport = "4111";
        public const string Telecom = "4814";
        public const string Utilities = "4900";
        public const string Hotel = "7011";
        public const string BeautyServices = "7298";
        public const string TaxiAndRideHailing = "4112";
        public const string Banking = "6012";
        public const string DepartmentStore = "5311";
        public const string GasStation = "5541";
        public const string ElectronicsStore = "5732";
        public const string JewelryStore = "5944";
        public const string GymAndFitness = "7997";
        public const string MoneyTransfer = "4829";
        public const string ToyStore = "5945";

        // Mapping từ Code sang Name
        private static readonly Dictionary<string, string> CodeToName = new()
        {
            [Unknown] = "Chưa xác định ngành nghề",
            [GroceryStores] = "Siêu thị, tạp hóa",
            [Restaurant] = "Nhà hàng, quán ăn",
            [FastFood] = "Đồ ăn nhanh",
            [Pharmacy] = "Nhà thuốc, hiệu thuốc",
            [MiscRetail] = "Cửa hàng bán lẻ khác",
            [FinancialServices] = "Dịch vụ tài chính (ATM, rút tiền)",
            [LocalTransport] = "Giao thông công cộng (bus, tàu điện)",
            [Telecom] = "Dịch vụ viễn thông, điện thoại",
            [Utilities] = "Dịch vụ điện, nước, gas",
            [Hotel] = "Khách sạn, nghỉ dưỡng",
            [BeautyServices] = "Dịch vụ làm đẹp, spa",
            [TaxiAndRideHailing] = "Taxi, dịch vụ xe công nghệ",
            [Banking] = "Tài chính, ngân hàng (chuyển khoản, thanh toán)",
            [DepartmentStore] = "Cửa hàng bách hóa",
            [GasStation] = "Trạm xăng, nhiên liệu",
            [ElectronicsStore] = "Cửa hàng điện tử, máy tính",
            [JewelryStore] = "Cửa hàng đồng hồ, trang sức",
            [GymAndFitness] = "Phòng gym, trung tâm thể dục",
            [MoneyTransfer] = "Dịch vụ chuyển tiền, ví điện tử",
            [ToyStore] = "Cửa hàng đồ chơi, trò chơi"
        };

        // Mapping ngược từ Name sang Code
        private static readonly Dictionary<string, string> NameToCode
            = CodeToName.ToDictionary(x => x.Value, x => x.Key);

        /// <summary>
        /// Lấy tên theo mã ngành nghề, trả về null nếu không tìm thấy.
        /// </summary>
        public static string? GetName(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return null;

            return CodeToName.TryGetValue(code, out var name) ? name : null;
        }

        /// <summary>
        /// Lấy mã theo tên ngành nghề, trả về null nếu không tìm thấy.
        /// </summary>
        public static string? GetCode(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            return NameToCode.TryGetValue(name, out var code) ? code : null;
        }

        /// <summary>
        /// Lấy toàn bộ danh sách mã ngành nghề cùng tên tương ứng.
        /// </summary>
        public static IReadOnlyDictionary<string, string> All => CodeToName;
    }
}
