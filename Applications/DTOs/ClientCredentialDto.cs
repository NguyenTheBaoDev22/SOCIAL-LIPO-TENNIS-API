namespace Applications.DTOs
{
    public class ClientCredentialDto
    {
        public Guid Id { get; set; }
        public string ClientId { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
