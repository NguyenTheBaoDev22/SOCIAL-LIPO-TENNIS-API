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
    public class UpdateShopInventoryCommand : IRequest<BaseResponse<bool>>
    {
        [Required]
        public Guid Id { get; set; }

        public DateTime AuditDate { get; set; }
        public int ExpectedQty { get; set; }
        public int ActualQty { get; set; }

        public string? Reason { get; set; }
        public string? AuditorName { get; set; }
    }

}
