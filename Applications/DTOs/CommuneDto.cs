namespace Applications.DTOs
{
    public class CommuneDto
    {
        // public Guid Id { get; set; }          // Mã định danh của Commune
        public string Code { get; set; }      // Mã Commune (Ví dụ: mã xã/phường)
        public string Name { get; set; }      // Tên Commune (Ví dụ: tên xã/phường)
        public string DivisionType { get; set; } // Loại phân chia (Ví dụ: Xã, Phường)
        public Guid ProvinceId { get; set; }
        // Bạn có thể thêm các thuộc tính khác tùy theo yêu cầu nghiệp vụ
    }
    public class CommuneRes
    {
        public Guid Id { get; set; }          // Mã định danh của Commune
        public string Code { get; set; }      // Mã Commune (Ví dụ: mã xã/phường)
        public string Name { get; set; }      // Tên Commune (Ví dụ: tên xã/phường)
        public string DivisionType { get; set; } // Loại phân chia (Ví dụ: Xã, Phường)
        public Guid ProvinceId { get; set; }
        // Bạn có thể thêm các thuộc tính khác tùy theo yêu cầu nghiệp vụ
    }
    public class CommuneInProvinceRes
    {
        public Guid Id { get; set; }          // Mã định danh của Commune
        public string Code { get; set; }      // Mã Commune (Ví dụ: mã xã/phường)
        public string Name { get; set; }      // Tên Commune (Ví dụ: tên xã/phường)
        public string DivisionType { get; set; } // Loại phân chia (Ví dụ: Xã, Phường)
    }
}
