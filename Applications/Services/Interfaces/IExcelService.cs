using Core.Entities;
using Microsoft.AspNetCore.Http;

namespace Applications.Services.Interfaces
{
    /// <summary>
    /// Interface cho service xử lý tệp Excel và chuyển dữ liệu vào hệ thống.
    /// </summary>
    public interface IExcelService
    {
        /// <summary>
        /// Phương thức để nhập dữ liệu từ tệp Excel và chuyển đổi thành danh sách các đối tượng Commune.
        /// </summary>
        /// <param name="file">Tệp Excel chứa dữ liệu.</param>
        /// <returns>Danh sách các Commune được trích xuất từ tệp Excel.</returns>
        Task<List<Commune>> ImportCommuneExcelDataAsync(IFormFile file);
    }
}
