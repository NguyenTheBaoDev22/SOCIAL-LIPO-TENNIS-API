using Applications.Services.Interfaces;
using Core.Enumerables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
   // [Authorize(Roles = RoleEnum.AdminDashboard)]
    public class ImportController : ControllerBase
    {
        private readonly IExcelService _excelService;
        private readonly IAdministrativeDataService _importService;
        public ImportController(IExcelService excelService, IAdministrativeDataService importService)//
        {
            _excelService = excelService;
            _importService = importService;
        }


        [HttpPost("import-excel")]
        public async Task<IActionResult> ImportExcel(IFormFile file)
        {
            var name = file.Name;
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            // Gọi service xử lý file
            var communes = await _excelService.ImportCommuneExcelDataAsync(file);

            if (communes != null && communes.Any())
                return Ok(communes); // Trả về dữ liệu đã xử lý thành công

            return BadRequest("Error while importing file.");

        }
        /// <summary>
        /// Import dữ liệu tỉnh/thành và xã/phường từ file VNLocations2025.txt
        /// </summary>
        [HttpPost("import-json-file")]
        public async Task<IActionResult> ImportJsonFile(IFormFile file)
        {
            var result = await _importService.ImportDataFromJsonFileAsync(file);
            return Ok(result);
        }
    }
}
