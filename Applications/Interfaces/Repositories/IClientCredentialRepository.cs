using Core.Entities;

namespace Applications.Interfaces.Repositories
{
    public interface IClientCredentialRepository : IBaseRepository<ClientCredential>
    {
        Task<ClientCredential?> GetByClientIdAsync(string clientId);
        Task<ClientCredential?> ValidateClientAsync(string clientId, string clientSecret);
    }

}
