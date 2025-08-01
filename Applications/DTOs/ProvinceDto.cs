namespace Applications.DTOs
{
    public class ProvinceDto
    {
        // public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string DivisionType { get; set; }
    }
    public class CreateProvinceReq
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string DivisionType { get; set; }
    }
    public class ProvinceRes
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string DivisionType { get; set; }
    }
    public class GetAllWithCommunesRes
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string DivisionType { get; set; }
        public ICollection<CommuneInProvinceRes> Communes { get; set; }



    }
}
