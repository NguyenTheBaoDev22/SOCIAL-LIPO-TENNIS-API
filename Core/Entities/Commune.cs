namespace Core.Entities
{
    public class Commune : Audit
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string DivisionType { get; set; }
        // Quan hệ với District (Quận/Huyện)
        public Guid ProvinceId { get; set; }  // Khóa ngoại đến Province
        public Province Province { get; set; } // Điều hướng tới Province
    }
}
