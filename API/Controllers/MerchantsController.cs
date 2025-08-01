using Applications.Features.Merchants.Commands;
using Applications.Features.Merchants.Commands.Applications.Features.Merchants.Commands;
using Applications.Features.Merchants.DTOs;
using Applications.Features.Merchants.Queries;
using Core.Entities;
using Core.Enumerables;
using Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Shared.Attributes;
using Shared.Authorization;
using Shared.Results;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize(Roles = RoleEnum.AdminDashboard)]
    public class MerchantsController : ControllerBase
    {
        private readonly IMediator _mediator;

        // Constructor để inject MediatR
        public MerchantsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST: api/Merchants
        [HttpPost]
      //  [Authorize(Roles = RoleEnum.AdminDashboard)]
        public async Task<ActionResult<MerchantDto>> CreateMerchant(CreateMerchantCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetMerchantById), new { id = result.Id }, result);
        }

        // GET: api/Merchants/{id}
        [HttpGet("{id}")]
     //   [Authorize(Roles = $"{RoleEnum.AdminDashboard}, {RoleEnum.MobileShopManager}")]
        public async Task<ActionResult<MerchantDto>> GetMerchantById(Guid id)
        {
            var result = await _mediator.Send(new GetMerchantByIdQuery { Id = id });
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        // PUT: api/Merchants/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<MerchantDto>> UpdateMerchant(Guid id, UpdateMerchantCommand command)
        {
            if (id != command.Id)
                return BadRequest();

            var result = await _mediator.Send(command);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // DELETE: api/Merchants/{id}
        [HttpDelete("{id}")]
       // [Authorize(Roles = RoleEnum.AdminDashboard)]
        public async Task<ActionResult> DeleteMerchant(Guid id)
        {
            var result = await _mediator.Send(new DeleteMerchantCommand { Id = id });
            if (!result)
                return NotFound();

            return NoContent();
        }

        // API: Đăng ký địa điểm kinh doanh với thông tin merchant mới
        [HttpPost("register")]
      //  [Authorize(Roles = $"{RoleEnum.AdminDashboard}, {RoleEnum.MobileShopManager}")]
        public async Task<IActionResult> RegisterMerchantWithBranch([FromBody] RegisterMerchantWithBranchCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);

            //Mockup response để test
            //var mockResult = BaseResponse<RegisterMerchantWithBranchRes>.Success(
            //    new RegisterMerchantWithBranchRes
            //    {
            //        MerchantId = Guid.NewGuid(),
            //        MerchantCode = "MOCK001",
            //        MerchantBranchId = Guid.NewGuid(),
            //        MerchantBranchCode = "BR001",
            //        IsMerchantBranchActive = true
            //    },
            //    "Merchant and Branch registered successfully (MOCK)",
            //    "00"
            //);

            //return Ok(mockResult);
        }
        // API: Đăng ký địa điểm kinh doanh với thông tin merchant mới
        [HttpPost("partner/register")]
        //  [Authorize(Roles = $"{RoleEnum.AdminDashboard}, {RoleEnum.MobileShopManager}")]
        public async Task<IActionResult> PartnerRegisterMerchantWithBranch([FromBody] RegisterMerchantWithBranchCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);

            //Mockup response để test
            //var mockResult = BaseResponse<RegisterMerchantWithBranchRes>.Success(
            //    new RegisterMerchantWithBranchRes
            //    {
            //        MerchantId = Guid.NewGuid(),
            //        MerchantCode = "MOCK001",
            //        MerchantBranchId = Guid.NewGuid(),
            //        MerchantBranchCode = "BR001",
            //        IsMerchantBranchActive = true
            //    },
            //    "Merchant and Branch registered successfully (MOCK)",
            //    "00"
            //);

            //return Ok(mockResult);
        }
        /// <summary>
        /// Thêm địa điểm bán hàng mới cho Merchant đã tồn tại.
        /// </summary>
        [HttpPost("add-branch")]
        //[Authorize(Roles = $"{RoleEnum.AdminDashboard}, {RoleEnum.MobileShopManager}")]
        public async Task<IActionResult> AddBranch([FromBody] AddMerchantBranchCommand req)
        {
            var result = await _mediator.Send(req);
            return Ok(result);
        }

        /// <summary>
        /// Tạo mới một thiết bị thanh toán (PaymentTerminal) cho chi nhánh merchant đã tồn tại.
        /// </summary>
        /// <param name="command">Thông tin thiết bị và merchant cần khai báo.</param>
        // /// <returns>Thông tin thiết bị vừa tạo.</returns>
        // [HttpPost("add-terminals")]
        //// [Authorize(Roles = $"{RoleEnum.AdminDashboard}, {RoleEnum.MobileShopManager}")]
        // public async Task<IActionResult> AddPaymentTerminal([FromBody] AddPaymentTerminalCommand command)
        // {
        //     var result = await _mediator.Send(command);
        //     return Ok(result);
        // }

        /// <summary>
        /// Thêm thiết bị thanh toán mới
        /// </summary>
        [HttpPost("add-terminals")]
        // [Authorize(Roles = $"{RoleEnum.AdminDashboard}, {RoleEnum.MobileShopManager}")]
        public async Task<IActionResult> AddPaymentTerminal([FromBody] AddPaymentTerminalCommand command)
        {
            // Trả về kết quả thành công với data là đối tượng AddPaymentTerminalCommand
            return Ok(BaseResponse<AddPaymentTerminalCommand>.Success(command));
        }


        [HttpPost("partner/approve")]
        //[Authorize(Roles = RoleEnum.AdminDashboard)]
        //[RequirePermission(PermissionCode.ApproveMerchant)] // OR mặc định
        //[RequirePermission(new[] { PermissionCode.CreateUser, PermissionCode.AssignRole }, PermissionMatchMode.And)]
      //  [RequirePermission(PermissionCode.ApproveMerchant)]
        public async Task<IActionResult> ApprovePartnerMerchant([FromBody] ApprovePartnerMerchantCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("approve")]
        [Authorize]
        [AuthorizePermission(PermissionCode.ApproveMerchant)]
        public async Task<IActionResult> ApproveMerchant([FromBody] ApproveMerchantCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        
    

        // API tra cứu mã số thuế (mock trực tiếp trong controller)

        /// <summary>
        /// Tra cứu thông tin mã số thuế từ VietQR API.
        /// </summary>

        [HttpGet("tax-info/lookup/{taxCode}")]
        [ProducesResponseType(typeof(BaseResponse<TaxCodeLookupResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> LookupTaxCode(string taxCode)
        {
            var result = await _mediator.Send(new LookupTaxCodeQuery(taxCode));
            return Ok(result);
        }




        // Lớp phản hồi mock
        public class TaxCodeLookupResponse
        {
            public string CompanyName { get; set; }
            public string CompanyAddress { get; set; }
        }


        // API lấy danh sách loại hình kinh doanh (mock trực tiếp trong controller)
        [HttpGet("business-types")]
        public ActionResult<BaseResponse<List<BusinessTypeResponse>>> GetBusinessTypes()
        {
            // Mock danh sách loại hình kinh doanh
            var result = BusinessTypeConstants.BusinessTypes
               .Select(bt => new BusinessTypeResponse
               {
                   BusinessTypeCode = bt.Code,
                   BusinessTypeName = bt.Name
               })
           .ToList();
            return Ok(BaseResponse<List<BusinessTypeResponse>>.Success(result));
        }
        // API lấy danh sách ngành nghề kinh doanh (mock trực tiếp trong controller)
        [HttpGet("merchant-categories")]
        public ActionResult<BaseResponse<List<MerchantCategoryResponse>>> GetMerchantCategories()
        {
            // Lấy danh sách ngành nghề kinh doanh từ MerchantCategoryCodes
            var categories = MerchantCategoryCodes.All
                .Select(item => new MerchantCategoryResponse
                {
                    MerchantCategoryCode = item.Key,  // Mã ngành nghề (ví dụ: "5411")
                    MerchantCategoryName = item.Value // Tên ngành nghề (ví dụ: "Siêu thị, tạp hóa")
                })
                .ToList();

            return Ok(BaseResponse<List<MerchantCategoryResponse>>.Success(categories));
        }
        public class MerchantCategoryResponse
        {
            public string MerchantCategoryCode { get; set; }
            public string MerchantCategoryName { get; set; }
        }




        // API lấy danh sách ngân hàng (mock trực tiếp trong controller)
        [HttpGet("bank-info")]
        public ActionResult<BaseResponse<List<BankInfoResponse>>> GetBanks()
        {
            // Lấy danh sách các ngân hàng từ BankConstants
            var bankList = BankConstants.BankList.Select(b => new BankInfoResponse
            {
                BankName = b.BankName,
                BankCode = b.BankCode,
                Bin = b.Bin
            }).ToList();

            return Ok(BaseResponse<List<BankInfoResponse>>.Success(bankList));
        }
        public class BankInfoResponse
        {
            public string BankName { get; set; }
            public string BankCode { get; set; }
            public string Bin { get; set; }
        }

        /// <summary>
        /// Lấy danh sách thiết bị thanh toán theo MerchantBranchId.
        /// </summary>
        [HttpGet("get-terminals")]
        public ActionResult<BaseResponse<IEnumerable<PaymentTerminal>>> GetTerminals([FromQuery] Guid merchantBranchId)
        {
            // Mock danh sách các thiết bị thanh toán cho merchantBranchId cụ thể
            var paymentTerminals = new List<PaymentTerminal>
            {
                new PaymentTerminal
                {
                    Id = Guid.NewGuid(),
                    MerchantBranchId = merchantBranchId,
                    TerminalCode = "01",
                    TerminalName = "Máy POS Quầy 1",
                    DeviceType = "POS",
                    SerialNumber = "SN12345",
                    IMEI = "IMEI12345",
                    Manufacturer = "Pax",
                    Model = "A920",
                    Status = "Active",
                    MerchantId = Guid.NewGuid(),
                    MerchantCode = "MERCHANT01",
                    MerchantBranchCode = "BRANCH01",
                    CombinedIdentifier = "MERCHANT01-BRANCH01-01",
                    FirmwareVersion = "1.0.0",
                    LastSyncDate = DateTime.UtcNow,
                    DeviceId = "192.168.1.1",
                    CreatedAt = DateTime.UtcNow
                },
                new PaymentTerminal
                {
                    Id = Guid.NewGuid(),
                    MerchantBranchId = merchantBranchId,
                    TerminalCode = "02",
                    TerminalName = "Soundbox Quầy 2",
                    DeviceType = "Soundbox",
                    SerialNumber = "SN67890",
                    IMEI = "IMEI67890",
                    Manufacturer = "Ingenico",
                    Model = "P400",
                    Status = "Inactive",
                    MerchantId = Guid.NewGuid(),
                    MerchantCode = "MERCHANT02",
                    MerchantBranchCode = "BRANCH02",
                    CombinedIdentifier = "MERCHANT02-BRANCH02-02",
                    FirmwareVersion = "1.1.0",
                    LastSyncDate = DateTime.UtcNow,
                    DeviceId = "192.168.1.2",
                    CreatedAt = DateTime.UtcNow
                }
            };

            // Trả về kết quả thành công với danh sách các thiết bị thanh toán
            return Ok(BaseResponse<IEnumerable<PaymentTerminal>>.Success(paymentTerminals));
        }






    }

}
