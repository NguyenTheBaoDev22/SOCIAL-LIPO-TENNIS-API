using MediatR;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.Shops.Products.Commands
{
    public class UpdateProductCategoryCommand : IRequest<BaseResponse<bool>>
    {
        [Required]
        public Guid Id { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; } = default!;
    }
}
