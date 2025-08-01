using MediatR;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.Shops.Inventories.Commands
{
    public class CreateShopInventoryCommand : IRequest<BaseResponse<Guid>>
    {
        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public DateTime AuditDate { get; set; }

        public int ExpectedQty { get; set; }
        public int ActualQty { get; set; }

        [MaxLength(500)]
        public string? Reason { get; set; }

        [MaxLength(255)]
        public string? AuditorName { get; set; }
    }
}
