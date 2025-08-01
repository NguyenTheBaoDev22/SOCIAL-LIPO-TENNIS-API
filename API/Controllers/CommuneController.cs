using Applications.DTOs;
using Applications.Services.Interfaces;
using Core.Enumerables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
  //  [Authorize(Roles = RoleEnum.AdminDashboard)]
    public class CommuneController : ControllerBase
    {
        private readonly ICommuneService _communeService;
        //  private readonly IExcelService _excelService;
        public CommuneController(ICommuneService communeService)//, IExcelService excelService
        {
            _communeService = communeService;
            // _excelService = excelService;
        }

        // Lấy thông tin Commune theo ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var response = await _communeService.GetByIdAsync(id);
            if (response.IsSuccess)
                return Ok(response.Data);

            return NotFound(response.Message);
        }

        // Lấy danh sách tất cả các Commune
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _communeService.GetAllPaginatedAsync();
            if (response.IsSuccess)
                return Ok(response);

            return NotFound(response);
        }

        // Thêm mới Commune
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CommuneDto communeDto)
        {
            var response = await _communeService.CreateAsync(communeDto);
            if (response.IsSuccess)
                return Ok(response.Data);

            return BadRequest(response.Message);
        }
        [HttpPost("create-many")]
        public async Task<IActionResult> CreateMany([FromBody] IEnumerable<CommuneDto> communeDtos)
        {
            var response = await _communeService.CreateManyAsync(communeDtos);

            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response);
        }
        // Cập nhật thông tin Commune
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] CommuneDto communeDto)
        {
            var response = await _communeService.UpdateAsync(id, communeDto);
            if (response.IsSuccess)
                return Ok(response.Data);

            return NotFound(response.Message);
        }

        // Xóa Commune
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var response = await _communeService.DeleteAsync(id);
            if (response.IsSuccess)
                return Ok(response.Data);

            return NotFound(response.Message);
        }

        // Tìm kiếm Commune theo mã
        [HttpGet("search/{code}")]
        public async Task<IActionResult> GetByCodeAsync(string code)
        {
            var response = await _communeService.GetByCodeAsync(code);
            if (response.IsSuccess)
                return Ok(response.Data);

            return NotFound(response.Message);
        }

        //// Endpoint để import Excel
        //[HttpPost("import-excel")]
        //public async Task<IActionResult> ImportExcel([FromForm] IFormFile file)
        //{
        //    try
        //    {
        //        if (file == null || file.Length == 0)
        //            return BadRequest("No file uploaded.");

        //        // Gọi phương thức từ ExcelService để xử lý file Excel và lưu dữ liệu
        //        var communes = await _excelService.ImportCommuneExcelDataAsync(file);

        //        // Nếu việc xử lý thành công, trả về dữ liệu đã xử lý
        //        if (communes.Count > 0)
        //            return Ok(new { message = "Data imported successfully.", data = communes });

        //        // Trả về lỗi nếu không có commune nào được xử lý
        //        return BadRequest("No valid data found in the Excel file.");
        //    }
        //    catch (Exception ex)
        //    {
        //        // Xử lý lỗi và trả về lỗi server nếu có
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}
    }
}
