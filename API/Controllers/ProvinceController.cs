using Applications.DTOs;
using Applications.Services.Interfaces;
using Core.Enumerables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = RoleEnum.AdminDashboard)]
    public class ProvinceController : ControllerBase
    {
        private readonly IProvinceService _provinceService;

        public ProvinceController(IProvinceService provinceService)
        {
            _provinceService = provinceService;
        }

        // Lấy thông tin Province theo ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var response = await _provinceService.GetByIdAsync(id);
            if (response.IsSuccess)
                return Ok(response.Data);

            return NotFound(response.Message);
        }

        // Lấy danh sách tất cả các Province
        [HttpGet]
        public async Task<IActionResult> GetAllAsync(int pageIndex = 1, int pageSize = 20)
        {
            var response = await _provinceService.GetAllPaginatedAsync(pageIndex, pageSize);
            if (response.IsSuccess)
                return Ok(response);

            return NotFound(response.Message);
        }

        // Thêm mới Province
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProvinceDto provinceDto)
        {
            var response = await _provinceService.CreateAsync(provinceDto);
            return Ok(response);
        }
        [HttpPost("create-many")]
        public async Task<IActionResult> CreateMany([FromBody] IEnumerable<ProvinceDto> provinceDtos)
        {
            var response = await _provinceService.CreateManyAsync(provinceDtos);

            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response);
        }
        // Cập nhật thông tin Province
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] ProvinceDto provinceDto)
        {
            var response = await _provinceService.UpdateAsync(id, provinceDto);
            if (response.IsSuccess)
                return Ok(response.Data);

            return NotFound(response.Message);
        }

        // Xóa Province
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var response = await _provinceService.DeleteAsync(id);
            if (response.IsSuccess)
                return Ok(response.Data);

            return NotFound(response.Message);
        }

        // Tìm kiếm Province theo mã
        [HttpGet("search/{code}")]
        public async Task<IActionResult> GetByCodeAsync(string code)
        {
            var response = await _provinceService.GetByCodeAsync(code);
            if (response.IsSuccess)
                return Ok(response.Data);

            return NotFound(response.Message);
        }
        [HttpGet("with-communes")]
        public async Task<IActionResult> GetAllWithCommunesAsync(int pageIndex = 1, int pageSize = 20)
        {
            var response = await _provinceService.GetAllWithCommunesAsync(pageIndex, pageSize);
            if (response.IsSuccess)
                return Ok(response);

            return NotFound(response.Message);
        }


    }
}
