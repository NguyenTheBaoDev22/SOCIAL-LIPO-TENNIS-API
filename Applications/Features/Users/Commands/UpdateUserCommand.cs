using MediatR;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.Users.Commands
{
    public class UpdateUserCommand : IRequest<BaseResponse<string>>
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public string Username { get; set; } = default!;

        [Required]
        public string PhoneNumber { get; set; } = default!;

        public bool? IsActive { get; set; }
        public string? NewPassword { get; set; }
    }
}
