using Applications.Features.MerchantBranches.Commands;
using Applications.Features.MerchantBranches.Dtos;
using Applications.Features.MerchantBranches.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Results;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //  [Authorize(Roles = RoleEnum.AdminDashboard)]
    public class MerchantBranchesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MerchantBranchesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMerchantBranch([FromBody] CreateMerchantBranchCommand command)
        {
            // Kiểm tra nếu dữ liệu không hợp lệ
            if (command == null)
            {
                return BadRequest(BaseResponse<CreateMerchantBranchRes>.Error("Dữ liệu không hợp lệ."));
            }

            // Gửi command để xử lý tạo mới chi nhánh
            var result = await _mediator.Send(command);

            // Trả về kết quả
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponse<MerchantBranchDto>>> GetMerchantBranchById(Guid id)
        {
            // Gửi query để lấy MerchantBranch từ DB
            var result = await _mediator.Send(new GetMerchantBranchByIdQuery { Id = id });

            // Nếu không tìm thấy, trả về phản hồi lỗi
            if (result == null)
            {
                return NotFound(BaseResponse<MerchantBranchDto>.Error("Merchant branch not found."));
            }

            // Nếu tìm thấy, trả về dữ liệu thành công với BaseResponse
            return Ok(BaseResponse<MerchantBranchDto>.Success(result));
        }
        //[HttpPut("{id}")]
        //public async Task<ActionResult<MerchantBranchDto>> UpdateMerchantBranch(Guid id, [FromBody] UpdateMerchantBranchCommand command)
        //{
        //    if (id != command.Id)
        //        return BadRequest("ID không khớp");

        //    var result = await _mediator.Send(command);
        //    if (result == null)
        //        return NotFound();
        //    return Ok(result);
        //}

        //[HttpDelete("{id}")]
        //public async Task<ActionResult> DeleteMerchantBranch(Guid id)
        //{
        //    var success = await _mediator.Send(new DeleteMerchantBranchCommand { Id = id });
        //    if (!success)
        //        return NotFound();
        //    return NoContent();
        //}

        // API Get Merchant Branch Status
        [HttpGet("status")]
        public async Task<ActionResult<BaseResponse<MerchantBranchStatusRes>>> GetStatus(Guid merchantBranchId)
        {
            try
            {
                // Gửi Query đến MediatR và nhận kết quả
                var query = new GetMerchantBranchStatusQuery(merchantBranchId);
                var result = await _mediator.Send(query);

                if (!result.IsSuccess)
                {
                    return NotFound(result); // Nếu không thành công, trả về thông báo lỗi
                }

                return Ok(result); // Trả về kết quả thành công
            }
            catch (Exception ex)
            {
                // Xử lý lỗi (ví dụ: ghi log exception)
                return StatusCode(500, BaseResponse<MerchantBranchStatusRes>.Error($"Internal server error: {ex.Message}", "500"));
            }
        }

        [HttpPut("activate")]
        public async Task<ActionResult<MerchantBranchDto>> ActivateMerchantBranch([FromBody] ActivateMerchantBranchCommand command)
        {
            var result = await _mediator.Send(command);
            if (result == null)
                return NotFound();

            return Ok(result);
        }


        [HttpGet("by-tax-number")]
        [ProducesResponseType(typeof(BaseResponse<GetMerchantBranchByTaxNumberRes>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByTaxNumber([FromQuery] string taxNumber, CancellationToken cancellationToken)
        {
            var query = new GetMerchantBranchByTaxNumberQuery { TaxNumber = taxNumber };
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result); // đảm bảo response trả ra BaseResponse<T>
        }

    }

}

