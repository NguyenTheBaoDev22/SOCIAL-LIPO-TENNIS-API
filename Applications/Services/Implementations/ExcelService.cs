using Applications.Interfaces.Repositories;
using Applications.Services.Interfaces;
using Core.Entities;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;

namespace Applications.Services.Implementations
{
    public class ExcelService : IExcelService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ExcelService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Commune>> ImportCommuneExcelDataAsync(IFormFile file)
        {
            var communes = new List<Commune>();

            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is empty or null.");
            }

            // Tạo từ điển để lưu ProvinceCode -> ProvinceId
            var provinceDictionary = await GetProvinceDictionaryAsync();

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 1; row <= rowCount; row++)
                    {
                        var provinceCode = worksheet.Cells[row, 4].Text;
                        var provinceId = provinceDictionary.ContainsKey(provinceCode)
                            ? provinceDictionary[provinceCode]
                            : Guid.Empty;  // Sử dụng Guid.Empty thay vì null

                        if (provinceId == Guid.Empty)
                        {
                            // Log lỗi nếu provinceId không hợp lệ
                            Console.WriteLine($"Invalid ProvinceCode: {provinceCode} at row {row}");
                            continue; // Bỏ qua commune này nếu provinceId không hợp lệ
                        }

                        var commune = new Commune
                        {
                            Code = worksheet.Cells[row, 1].Text,
                            Name = worksheet.Cells[row, 2].Text,
                            DivisionType = worksheet.Cells[row, 5].Text,
                            ProvinceId = provinceId // Gán ProvinceId vào commune
                        };

                        await _unitOfWork.CommuneRepositories.AddAsync(commune);
                        communes.Add(commune);
                    }
                }
            }

            // Lưu thay đổi vào cơ sở dữ liệu
            await _unitOfWork.SaveChangesAsync();

            return communes;
        }

        // Phương thức để tải ProvinceCode -> ProvinceId từ cơ sở dữ liệu vào bộ nhớ (dictionary)
        private async Task<Dictionary<string, Guid>> GetProvinceDictionaryAsync()
        {
            var provinces = await _unitOfWork.ProvinceRepositories.GetAllAsync();
            return provinces.ToDictionary(p => p.Code, p => p.Id);
        }
    }
}
