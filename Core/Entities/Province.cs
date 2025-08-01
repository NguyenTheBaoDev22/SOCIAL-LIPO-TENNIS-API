namespace Core.Entities
{
    public class Province : Audit
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string DivisionType { get; set; }
        // Quan hệ với các Commune (Xã/Phường)
        public ICollection<Commune> Communes { get; set; }  // Một quận có nhiều xã/phường
    }
}
