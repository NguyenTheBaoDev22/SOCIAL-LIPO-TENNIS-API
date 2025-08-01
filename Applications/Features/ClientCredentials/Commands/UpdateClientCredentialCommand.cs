using MediatR;
using Shared.Results;

namespace Applications.Features.ClientCredentials.Commands
{
    public class UpdateClientCredentialCommand : IRequest<BaseResponse<bool>>
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
